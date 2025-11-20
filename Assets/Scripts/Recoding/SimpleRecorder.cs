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
        Debug.Log("[SimpleRecorder] Start() 호출됨");

        // 현재 Unity가 인식한 마이크 목록 찍어보기
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("[SimpleRecorder] Microphone.devices가 0개입니다.");
        }
        else
        {
            Debug.Log("[SimpleRecorder] 사용 가능한 마이크 목록:");
            foreach (var d in Microphone.devices)
            {
                Debug.Log(" - " + d);
            }
        }
        // 마이크 연결 여부 확인 로직 추후 삭제 필요

        UpdateTimerText(recordSeconds);

        if (recordButton != null)
        {
            Debug.Log("[SimpleRecorder] Start() 호출됨");
            recordButton.onClick.AddListener(OnClickRecordButton);
        }

        UpdateTimerText(recordSeconds);
    }

    public void OnClickRecordButton()
    {
        Debug.Log("[SimpleRecorder] OnClickRecordButton 호출됨");

        if (isRecording) return;

        bool hasMic = Microphone.devices != null && Microphone.devices.Length > 0;

        if (!hasMic)
        {
            Debug.LogWarning("[SimpleRecorder] 마이크가 없어서 타이머만 진행합니다.");
            if (instructionText != null)
                instructionText.text = "마이크가 없어 타이머만 진행합니다.";

            isRecording = true;
            recordButton.interactable = false;
            StartCoroutine(RecordTimerCoroutine());
            return;
        }

        // 여기까지 왔다는 건 마이크가 최소 1개 있다는 뜻
        string deviceName = Microphone.devices[0];
        Debug.Log("[SimpleRecorder] 사용 마이크: " + deviceName);

        isRecording = true;
        recordButton.interactable = false;

        if (instructionText != null)
            instructionText.text = "녹음 중입니다...";

        recordedClip = Microphone.Start(deviceName, false, recordSeconds, 44100);


        // ✅ 마이크 관련 코드 전부 잠시 주석 처리!
        // if (Microphone.devices.Length == 0) { ... }
        // recordedClip = Microphone.Start(...);

        StartCoroutine(RecordTimerCoroutine());
        // Debug.Log("[SimpleRecorder] OnClickRecordButton 호출됨"); // 로그 추후 지우기

        // // 마이크 체크는 잠시 대충만 로그 찍고, return 은 막자
        // if (Microphone.devices.Length == 0)
        // {
        //     // 마이크 연결 확인하는 로직
        //     Debug.LogError("[SimpleRecorder] 클릭 시점에도 마이크 0개");
        //     if (instructionText != null)
        //         instructionText.text = "마이크를 찾을 수 없습니다.";
        //     // return;
        // }

        // string deviceName = Microphone.devices[0];
        // Debug.Log("[SimpleRecorder] 사용 마이크: " + deviceName);
        // // 여기까지

        // if (isRecording) return;

        // if (Microphone.devices.Length == 0)
        // {
        //     Debug.LogError("No microphone detected!");
        //     if (instructionText != null)
        //         instructionText.text = "마이크를 찾을 수 없습니다.";
        //     // 타이머 테스트용 추후 주석 해제 필요
        //     //return;
        // }

        // isRecording = true;
        // recordButton.interactable = false;

        // if (instructionText != null)
        //     instructionText.text = "녹음 중입니다...";

        // // 마이크 시작
        // //recordedClip = Microphone.Start(null, false, recordSeconds, 44100);
        // // ↑ 일단 이 줄도 잠깐 막아도 됨 (녹음 없이 타이머만 테스트용)

        // // 타이머 시작
        // StartCoroutine(RecordTimerCoroutine());
    }

    IEnumerator RecordTimerCoroutine()
    {
        Debug.Log("[SimpleRecorder] RecordTimerCoroutine 진입");

        int remaining = recordSeconds;
        Debug.Log("[SimpleRecorder] 초기 remaining = " + remaining);

        while (remaining > 0)
        {
            Debug.Log("[SimpleRecorder] 남은 시간: " + remaining);
            UpdateTimerText(remaining);
            yield return new WaitForSeconds(1f);
            remaining--;
        }

        UpdateTimerText(0);
        Debug.Log("[SimpleRecorder] 타이머 종료, 0초 도달");

        // 일단 테스트 단계에선 SaveRecording() 도 잠시 꺼놔도 됨
        // SaveRecording();

        if (fadeController != null)
        {
            fadeController.StartFadeOut();
        }

        isRecording = false;
        // Debug.Log("[SimpleRecorder] RecordTimerCoroutine 진입");

        // int remaining = recordSeconds;
        // Debug.Log("[SimpleRecorder] 초기 remaining = " + remaining);


        // while (remaining > 0)
        // {
        //     Debug.Log("[SimpleRecorder] 남은 시간: " + remaining);
        //     UpdateTimerText(remaining);
        //     yield return new WaitForSeconds(1f);
        //     remaining--;
        // }

        UpdateTimerText(0);
        Debug.Log("[SimpleRecorder] 타이머 종료, 0초 도달");

        // // 녹음 종료
        // Microphone.End(null);
        // isRecording = false;

        // // 안내 문구 변경
        // if (instructionText != null)
        //     instructionText.text = "녹음이 완료되었습니다.";

        // 파일 저장
        SaveRecording();

        // 페이드 아웃 시작
        if (fadeController != null)
        {
            yield return fadeController.StartFadeOutCoroutine();
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

        string folder = Path.Combine(Application.persistentDataPath, "Recordings");
        Directory.CreateDirectory(folder);

        string fileName = $"recording_dummy_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt";
        string filePath = Path.Combine(folder, fileName);

        File.WriteAllText(filePath, "dummy recording (no mic available)");

        Debug.Log("[SimpleRecorder] 더미 녹음 파일 생성됨: " + filePath);
        //     if (recordedClip == null)
        //     {
        //         Debug.LogError("SimpleRecorder: recordedClip is null, cannot save.");
        //         return;
        //     }

        //     // 저장 경로: 앱의 persistentDataPath 내부
        //     string folder = Path.Combine(Application.persistentDataPath, "Recordings");
        //     Directory.CreateDirectory(folder);

        //     string fileName = $"recording_{System.DateTime.Now:yyyyMMdd_HHmmss}.wav";
        //     string filePath = Path.Combine(folder, fileName);

        //     WavUtility.Save(filePath, recordedClip);

        //     if (instructionText != null)
        //         instructionText.text += $"\n저장 위치: {filePath}";
    }
}
