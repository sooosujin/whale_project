using UnityEngine;

public class WhaleCameraDolly : MonoBehaviour
{
    [Header("카메라가 도착할 위치")]
    public Transform moveTarget;   // CameraTarget

    [Header("카메라가 바라볼 대상 (고래 머리 등)")]
    public Transform lookTarget;   // WhaleTarget (또는 고래 Transform)

    [Header("이동 시간 (초)")]
    public float duration = 15f;

    [Header("씬 시작과 함께 자동 재생")]
    public bool playOnStart = true;

    private Vector3 startPos;
    private float timer = 0f;
    private bool isPlaying = false;

    void Start()
    {
        if (playOnStart)
        {
            Begin();
        }
    }

    public void Begin()
    {
        if (moveTarget == null)
        {
            Debug.LogWarning("[WhaleCameraDolly] moveTarget이 비어 있습니다.");
            return;
        }

        startPos = transform.position;
        timer = 0f;
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying || moveTarget == null)
            return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        // 카메라 위치 보간
        transform.position = Vector3.Lerp(startPos, moveTarget.position, t);

        // 항상 고래(또는 WhaleTarget)를 바라보기
        if (lookTarget != null)
        {
            transform.LookAt(lookTarget.position);
        }

        if (t >= 1f)
        {
            isPlaying = false;
        }
    }
}
