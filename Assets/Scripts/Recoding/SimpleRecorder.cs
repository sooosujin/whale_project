using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.IO;

public class SimpleRecorder : MonoBehaviour
{
    [Header("UI 연결")]
    public Button recordButton;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI instructionText;

    [Header("페이드 컨트롤러")]
    public FadeController fadeController;

    [Header("녹음 시간 (초)")]
    public int recordSeconds = 60;

    private AudioClip recordedClip;
    private bool isRecording = false;

    private void Start()
    {

        if (recordButton != null)
        {
            Debug.Log("[SimpleRecorder] Start() 호출됨");
            recordButton.onClick.AddListener(OnClickRecordButton);
        }

        UpdateTimerText(recordSeconds);
    }

    public void OnClickRecordButton()
    {
        Debug.Log("[SimpleRecorder] OnClickRecordButton 호출됨"); // 로그 추후 지우기

        if (isRecording) return;

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            if (instructionText != null)
                instructionText.text = "마이크를 찾을 수 없습니다.";
            return;
        }

        isRecording = true;
        recordButton.interactable = false;

        if (instructionText != null)
            instructionText.text = "녹음 중입니다...";

        // 마이크 시작
        recordedClip = Microphone.Start(null, false, recordSeconds, 44100);

        // 타이머 시작
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

        UpdateTimerText(0);

        // 녹음 종료
        Microphone.End(null);
        isRecording = false;

        // 안내 문구 변경
        if (instructionText != null)
            instructionText.text = "녹음이 완료되었습니다.";

        // 파일 저장
        SaveRecording();

        // 페이드 아웃 시작
        if (fadeController != null)
        {
            fadeController.StartFadeOut();
        }
    }

    void UpdateTimerText(int seconds)
    {
        if (timerText == null) return;

        int m = seconds / 60;
        int s = seconds % 60;
        timerText.text = $"{m:00}:{s:00}";
    }

    void SaveRecording()
    {
        if (recordedClip == null)
        {
            Debug.LogError("SimpleRecorder: recordedClip is null, cannot save.");
            return;
        }

        // 저장 경로: 앱의 persistentDataPath 내부
        string folder = Path.Combine(Application.persistentDataPath, "Recordings");
        Directory.CreateDirectory(folder);

        string fileName = $"recording_{System.DateTime.Now:yyyyMMdd_HHmmss}.wav";
        string filePath = Path.Combine(folder, fileName);

        WavUtility.Save(filePath, recordedClip);

        if (instructionText != null)
            instructionText.text += $"\n저장 위치: {filePath}";
    }
}
