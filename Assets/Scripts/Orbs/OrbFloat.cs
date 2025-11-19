using UnityEngine;

public class OrbFloat : MonoBehaviour
{
    [Header("떠오르는 속도 (유닛/초)")]
    public float floatSpeed = 0.5f;

    [Header("생존 시간 (초)")]
    public float lifeTime = 20f;

    private float timer = 0f;

    void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime, Space.World);

        // 생존 시간 체크
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
