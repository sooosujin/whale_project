using UnityEngine;

public class CameraApproach : MonoBehaviour
{
    public Transform whale;
    public float moveSpeed = 0.5f;
    public float minDistance = 5f;

    void LateUpdate()
    {
        if (whale == null) return;

        float distance = Vector3.Distance(transform.position, whale.position);

        if (distance > minDistance)
        {
            // 카메라의 '앞 방향'으로 이동
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
