using UnityEngine;

public class WhaleCameraSimple : MonoBehaviour
{
    [Header("따라갈 고래")]
    public Transform whale;

    [Header("고래 기준 카메라 위치 오프셋")]
    public Vector3 startOffset = new Vector3(0f, 2f, -20f);
    public Vector3 endOffset = new Vector3(0f, 2f, -8f);

    [Header("다가가는 데 걸리는 시간 (초)")]
    public float moveDuration = 20f;

    private float timer = 0f;

    void Start()
    {
        if (whale == null) return;

        // 시작 위치 한번 세팅
        transform.position = whale.position + startOffset;
    }

    void LateUpdate()
    {
        if (whale == null) return;

        // 0 → 1로 서서히 증가
        if (moveDuration > 0f)
        {
            timer += Time.deltaTime;
        }
        float t = Mathf.Clamp01(timer / moveDuration);

        // 카메라 위치 보간
        Vector3 currentOffset = Vector3.Lerp(startOffset, endOffset, t);
        transform.position = whale.position + currentOffset;

        // 항상 고래를 바라보게
        Vector3 lookTarget = whale.position + Vector3.up * 0.5f; // 살짝 위를 보게
        transform.LookAt(lookTarget);
    }
}
