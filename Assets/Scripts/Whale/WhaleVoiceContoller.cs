using UnityEngine;
using System.Collections;

public class WhaleVoiceController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip whaleClip;

    [Tooltip("고래 소리 최소 간격 (초)")]
    public float minInterval = 5f;

    [Tooltip("고래 소리 최대 간격 (초)")]
    public float maxInterval = 10f;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError("WhaleVoiceController: AudioSource is missing!");
            return;
        }

        if (whaleClip == null)
        {
            Debug.LogError("WhaleVoiceController: Whale clip is not assigned!");
            return;
        }

        StartCoroutine(PlayWhaleVoiceRoutine());
    }

    IEnumerator PlayWhaleVoiceRoutine()
    {
        while (true)
        {
            // 랜덤 간격 대기
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // 소리 재생
            audioSource.PlayOneShot(whaleClip);
        }
    }
}
