using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

[Serializable]
public class F0Sample
{
    public float timeSec;
    public float f0SmoothHz;
    public float value01;
    public bool voiced;
}

public class CsvF0Track
{
    public List<F0Sample> samples = new();
    public float Duration => samples.Count > 0 ? samples[^1].timeSec : 0f;

    public static CsvF0Track LoadFromStreamingAssets(string fileName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (!File.Exists(path)) { Debug.LogError($"CSV not found: {path}"); return null; }

        var track = new CsvF0Track();
        using var sr = new StreamReader(path);
        string header = sr.ReadLine(); // time_sec,f0_hz,f0_smooth_hz,value01,voiced
        string line; var inv = CultureInfo.InvariantCulture;

        while ((line = sr.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var t = line.Split(',');
            if (t.Length < 5) continue;

            track.samples.Add(new F0Sample {
                timeSec     = float.Parse(t[0], inv),
                f0SmoothHz  = float.Parse(t[2], inv),
                value01     = float.Parse(t[3], inv),
                voiced      = t[4].Trim() == "1" || t[4].Trim().ToLower() == "true"
            });
        }
        return track;
    }

    // 이진 탐색 + 선형보간으로 t(초) 시점 샘플 가져오기
    public F0Sample SampleAt(float t)
    {
        if (samples.Count == 0) return new F0Sample();
        if (t <= samples[0].timeSec) return samples[0];
        if (t >= samples[^1].timeSec) return samples[^1];

        int lo = 0, hi = samples.Count - 1;
        while (hi - lo > 1)
        {
            int mid = (lo + hi) / 2;
            if (samples[mid].timeSec < t) lo = mid; else hi = mid;
        }
        var a = samples[lo]; var b = samples[hi];
        float u = Mathf.InverseLerp(a.timeSec, b.timeSec, t);

        return new F0Sample {
            timeSec     = t,
            f0SmoothHz  = Mathf.Lerp(a.f0SmoothHz, b.f0SmoothHz, u),
            value01     = Mathf.Lerp(a.value01,    b.value01,    u),
            voiced      = (u < 0.5f) ? a.voiced : b.voiced
        };
    }
}
