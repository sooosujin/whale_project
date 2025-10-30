using UnityEngine;

public class WhaleController : MonoBehaviour
{
    public int beadThreshold = 15;
    public float riseHeight = 5f;
    public float riseDuration = 6f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0,1,1);

    Vector3 startPos;
    bool played;

    void Awake()
    {
        startPos = transform.position;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!played && BeadSpawner.TotalBeads >= beadThreshold)
        {
            played = true;
            gameObject.SetActive(true);
            StartCoroutine(RiseAndExit());
        }
    }

    System.Collections.IEnumerator RiseAndExit()
    {
        float t = 0f;
        Vector3 from = startPos;
        Vector3 to = startPos + Vector3.up * riseHeight;

        while (t < 1f)
        {
            t += Time.deltaTime / riseDuration;
            transform.position = Vector3.Lerp(from, to, ease.Evaluate(t));
            yield return null;
        }

        // 화면 밖으로 천천히 이동
        Vector3 end = to + new Vector3(20f, 5f, 0f);
        float d2 = 8f;
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / d2;
            transform.position = Vector3.Lerp(to, end, ease.Evaluate(t));
            yield return null;
        }
    }
}
