using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    [Header("References")]
    public GameObject orbPrefab;
    public Transform orbsParent;
    public Transform whale;

    [Header("Spawn Settings")]
    public int orbCount = 100;
    public float spawnRadius = 3f;      // 구슬이 흩어질 반경
    public float spawnDuration = 5f;    // 몇 초 동안 점점 채워지는 느낌

    [Header("Gather Settings")]
    public float gatherRadius = 1.5f;   // 고래 주변으로 모이는 반경
    public float gatherDuration = 5f;

    [Header("Follow Settings")]
    public float followDuration = 5f;   // 고래를 따라가는 시간
    public float fadeOutDuration = 2f;

    private List<Transform> orbs = new List<Transform>();
    private bool isFollowingWhale = false;

    // ---------- 타임라인에서 호출할 함수들 ----------

    public void StartSpawn()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnOrbsRoutine());
    }

    public void StartGatherToWhale()
    {
        StopAllCoroutines();
        StartCoroutine(GatherRoutine());
    }

    public void StartFollowAndDisappear()
    {
        StopAllCoroutines();
        StartCoroutine(FollowAndDisappearRoutine());
    }

    // ---------- 코루틴 구현 ----------

    IEnumerator SpawnOrbsRoutine()
    {
        // 처음 시작할 때 기존 구슬 제거
        foreach (var o in orbs)
        {
            if (o != null) Destroy(o.gameObject);
        }
        orbs.Clear();

        float elapsed = 0f;
        int created = 0;

        while (elapsed < spawnDuration && created < orbCount)
        {
            elapsed += Time.deltaTime;

            // 시간에 비례해서 생성 개수 늘리기 (대충 선형)
            float t = elapsed / spawnDuration;
            int targetCount = Mathf.RoundToInt(orbCount * t);

            while (created < targetCount)
            {
                Vector3 randPos = Random.insideUnitSphere * spawnRadius;
                GameObject orb = Instantiate(orbPrefab, orbsParent);
                orb.transform.localPosition = randPos;

                orbs.Add(orb.transform);
                created++;
            }

            yield return null;
        }
    }

    IEnumerator GatherRoutine()
    {
        if (whale == null || orbs.Count == 0) yield break;

        // 각 구슬의 시작 위치 / 목표 위치 기록
        Vector3[] startPos = new Vector3[orbs.Count];
        Vector3[] targetPos = new Vector3[orbs.Count];

        for (int i = 0; i < orbs.Count; i++)
        {
            if (orbs[i] == null) continue;

            startPos[i] = orbs[i].position;

            // 고래 주변 랜덤 위치 (고래 위치 + 작은 구)
            Vector3 offset = Random.insideUnitSphere * gatherRadius;
            targetPos[i] = whale.position + offset;
        }

        float elapsed = 0f;

        while (elapsed < gatherDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / gatherDuration);

            for (int i = 0; i < orbs.Count; i++)
            {
                if (orbs[i] == null) continue;
                orbs[i].position = Vector3.Lerp(startPos[i], targetPos[i], t);
            }

            yield return null;
        }
    }

    IEnumerator FollowAndDisappearRoutine()
    {
        if (whale == null || orbs.Count == 0) yield break;

        // 고래와의 상대 위치 기억
        Vector3[] localOffset = new Vector3[orbs.Count];
        for (int i = 0; i < orbs.Count; i++)
        {
            if (orbs[i] == null) continue;
            localOffset[i] = orbs[i].position - whale.position;
        }

        float elapsed = 0f;

        // 1단계: 고래를 따라감
        while (elapsed < followDuration)
        {
            elapsed += Time.deltaTime;

            for (int i = 0; i < orbs.Count; i++)
            {
                if (orbs[i] == null) continue;
                orbs[i].position = whale.position + localOffset[i];
            }

            yield return null;
        }

        // 2단계: 서서히 투명해지면서 삭제 (머티리얼이 투명 쉐이더라고 가정)
        elapsed = 0f;
        List<Renderer> renderers = new List<Renderer>();
        foreach (var o in orbs)
        {
            if (o != null)
            {
                var r = o.GetComponent<Renderer>();
                if (r != null) renderers.Add(r);
            }
        }

        // 알파 값 기억
        float[] startAlpha = new float[renderers.Count];
        for (int i = 0; i < renderers.Count; i++)
        {
            startAlpha[i] = renderers[i].material.color.a;
        }

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;

            for (int i = 0; i < renderers.Count; i++)
            {
                Color c = renderers[i].material.color;
                c.a = Mathf.Lerp(startAlpha[i], 0f, t);
                renderers[i].material.color = c;
            }

            yield return null;
        }

        // 완전히 삭제
        foreach (var o in orbs)
        {
            if (o != null) Destroy(o.gameObject);
        }
        orbs.Clear();
    }
}
