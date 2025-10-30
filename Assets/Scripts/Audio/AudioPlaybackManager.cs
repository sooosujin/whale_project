using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioLowPassFilter))]
public class AudioPlaybackManager : MonoBehaviour
{
 public float minInterval = 2f;
    public float maxInterval = 6f;
    public float volume = 0.6f;

    List<AudioClip> clips = new List<AudioClip>();
    AudioSource src;
    AudioLowPassFilter lp;

    void Awake()
    {
        src = gameObject.AddComponent<AudioSource>();
        src.spatialBlend = 0f;      // 스테레오
        src.loop = false;
        src.playOnAwake = false;
        src.volume = volume;

        lp = GetComponent<AudioLowPassFilter>();
        lp.cutoffFrequency = 200;   // 52Hz 느낌(저역 위주) 연출
    }

    // void Start()
    // {
    //     // CSV에서 wav 경로 읽어서 로드
    //     foreach (var path in DataLogger.LoadAllPaths("recordings.csv"))
    //     {
    //         var clip = WavUtility.ToAudioClip(path); // 간단한 WAV 로더 필요(아래 유틸)
    //         if (clip != null) clips.Add(clip);
    //     }
    //     if (clips.Count > 0) StartCoroutine(PlayLoop());
    // }

    // System.Collections.IEnumerator PlayLoop()
    // {
    //     var rnd = new System.Random();
    //     while (true)
    //     {
    //         if (!src.isPlaying && clips.Count > 0)
    //         {
    //             var clip = clips[rnd.Next(clips.Count)];
    //             src.clip = clip;
    //             src.pitch = 0.8f; // 느리게
    //             src.Play();
    //         }
    //         yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
    //     }
    // }
}
