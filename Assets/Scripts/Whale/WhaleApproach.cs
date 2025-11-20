using UnityEngine;

public class WhaleApproach : MonoBehaviour
{
    public Transform target;
    public float moveDuration = 10f;

    [Header("흔들림 옵션")]
    public float swayAmplitude = 0.3f;
    public float swayFrequency = 1f;

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        startPos = transform.position;

        if (target == null)
        {
            // 카메라 앞 z=3 지점으로 이동
            target = new GameObject("WhaleTarget").transform;
            target.position = new Vector3(0f, 0f, 3f);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / moveDuration);

        // 직선 이동
        Vector3 newPos = Vector3.Lerp(startPos, target.position, t);

        // 위아래 흔들림
        newPos.y += Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;

        transform.position = newPos;
    }
}
