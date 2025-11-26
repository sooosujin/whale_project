using UnityEngine;

public class WhaleCameraFollow : MonoBehaviour
{
    [Header("따라갈 고래")]
    public Transform whale;   // 고래(또는 아마튜어) Transform

    [Header("카메라 시작/끝 오프셋 (고래 기준)")]
    public Vector3 startOffset = new Vector3(0f, 2f, -25f); // 처음에는 멀리
    public Vector3 endOffset = new Vector3(0f, 2f, -10f); // 나중에는 조금 가까이

    [Header("고래에게 다가가는 데 걸리는 시간(초)")]
    public float moveDuration = 20f;

    private float elapsed = 0f;

    void LateUpdate()
    {
        if (whale == null) return;

        // 시간 누적
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / moveDuration); // 0 → 1

        // 오프셋을 서서히 바꾸기 (줌인 느낌)
        Vector3 currentOffset = Vector3.Lerp(startOffset, endOffset, t);

        // 카메라 위치 = 고래 위치 + 오프셋
        transform.position = whale.position + currentOffset;

        // 항상 고래를 바라보기
        transform.LookAt(whale.position);
    }
}
