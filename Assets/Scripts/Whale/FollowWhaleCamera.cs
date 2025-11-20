using UnityEngine;

public class FollowWhaleCamera : MonoBehaviour
{
    [Header("따라갈 고래")]
    public Transform whale;          // 고래(또는 아마튜어) Transform

    [Header("고래 기준 카메라 위치 오프셋")]
    public Vector3 offset = new Vector3(0f, 2f, -8f);

    void LateUpdate()
    {
        if (whale == null) return;

        // 고래 위치 기준으로 카메라 위치 설정
        transform.position = whale.position + offset;

        // 항상 고래를 바라보게
        transform.LookAt(whale.position);
    }
}
