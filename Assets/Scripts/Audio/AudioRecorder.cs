using System;
using System.IO;
using UnityEngine;


public class AudioRecorder : MonoBehaviour
{
  [Header("Mic")]
    [SerializeField] private string micDeviceName = null; // null -> default
    [SerializeField] private int sampleRate = 48000;
    [SerializeField] private int maxRecordSeconds = 60;

    [Header("Refs")]
    [SerializeField] private AudioSource monitorSource;

    public event Action<AudioClip> OnRecordingCompleted;

    private AudioClip _recording;
    private bool _isRecording;

    void Awake()
    {
        if (Microphone.devices.Length == 0)
            Debug.LogWarning("No microphone detected.");
    }

    public void StartRecording()
    {
        if (_isRecording) return;
        _isRecording = true;

        _recording = Microphone.Start(micDeviceName, false, maxRecordSeconds, sampleRate);
        if (monitorSource != null)
        {
            monitorSource.clip = _recording;
            monitorSource.loop = true;
            monitorSource.Play();
        }
    }

    public void StopRecording()
    {
        if (!_isRecording) return;
        _isRecording = false;

        int position = Microphone.GetPosition(micDeviceName);
        Microphone.End(micDeviceName);

        // Trim clip to actual length
        var samples = new float[position * _recording.channels];
        _recording.GetData(samples, 0);

        var trimmed = AudioClip.Create("Recorded", position, _recording.channels, sampleRate, false);
        trimmed.SetData(samples, 0);

        OnRecordingCompleted?.Invoke(trimmed);
    }

    public string SaveAudioWav(AudioClip clip, string fileNameNoExt = null)
    {
        if (clip == null) return null;

        string dir = Path.Combine(Application.persistentDataPath, "Recordings");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        string fileName = string.IsNullOrEmpty(fileNameNoExt) ? DateTime.Now.ToString("yyyyMMdd_HHmmss") : fileNameNoExt;
        string path = Path.Combine(dir, fileName + ".wav");

        //WavUtil.SaveWav(path, clip); // TODO: 구현 또는 임의 유틸 사용
        Debug.Log($"Saved: {path}");
        return path;
    }
}
