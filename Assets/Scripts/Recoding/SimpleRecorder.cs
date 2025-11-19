using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SimpleRecorder : MonoBehaviour
{
    public Button recordButton;
    public TextMeshProUGUI timerText;
    public Image fadeImage;

    public int recordSeconds = 60; // 1분
    private AudioClip recordedClip;
    private bool isRecording = false;

    private void Start()
    {
        recordButton.onClick.AddListener(StartRecording);
        UpdateTimerText(recordSeconds);
        SetFade(0f);
    }

    void StartRecording()
    {
        if (isRecording) return;

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }

        isRecording = true;
        recordButton.interactable = false;

        // 마이크 시작 (recordSeconds 길이 clip)
        recordedClip = Microphone.Start(null, false, recordSeconds, 44100);

        // 타이머 코루틴 시작
        StartCoroutine(RecordTimerCoroutine());
    }

    IEnumerator RecordTimerCoroutine()
    {
        int remaining = recordSeconds;

        while (remaining > 0)
        {
            UpdateTimerText(remaining);
            yield return new WaitForSeconds(1f);
            remaining--;
        }

        // 타이머 0초 표시
        UpdateTimerText(0);

        // 녹음 종료
        Microphone.End(null);
        isRecording = false;

        // 파일 저장
        SaveRecording();

        // 페이드 아웃 시작
        StartCoroutine(FadeOutCoroutine());
    }

    void UpdateTimerText(int seconds)
    {
        int m = seconds / 60;
        int s = seconds % 60;
        timerText.text = $"{m:00}:{s:00}";
    }

    void SaveRecording()
    {
        if (recordedClip == null)
        {
            Debug.LogError("No recorded clip to save!");
            return;
        }

        // 저장 경로 (테스트용)
        string filePath = System.IO.Path.Combine(
            Application.persistentDataPath,
            "user_recording.wav"
        );

        // TODO: 외부에서 가져온 WavUtility 사용
        // WavUtility.Save(filePath, recordedClip);
        Debug.Log("Saved recording to: " + filePath);
    }

    IEnumerator FadeOutCoroutine()
    {
        float duration = 2f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / duration);
            SetFade(normalized);
            yield return null;
        }
    }

    void SetFade(float alpha)
    {
        var color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
