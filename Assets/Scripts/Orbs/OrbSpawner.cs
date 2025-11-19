using UnityEngine;
using System.Collections;

public class OrbSpawner : MonoBehaviour
{
    [Header("구슬 프리팹")]
    public GameObject orbPrefab;

    [Header("스폰 영역 설정")]
    public Transform spawnAreaCenter;
    public Vector3 spawnAreaSize = new Vector3(5f, 0f, 5f);

    [Header("스폰 간격 (초)")]
    public float spawnInterval = 3f;

    private void Start()
    {
        if (orbPrefab == null)
        {
            Debug.LogError("OrbSpawner: Orb prefab is not assigned!");
            return;
        }

        if (spawnAreaCenter == null)
        {
            spawnAreaCenter = this.transform;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnOrb();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOrb()
    {
        // 스폰 영역 내 랜덤 위치 계산
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        Vector3 spawnPos = spawnAreaCenter.position + randomOffset;

        // 구슬 생성
        Instantiate(orbPrefab, spawnPos, Quaternion.identity);
    }

    // 필요시 씬에서 스폰 영역 확인용 기즈모
    private void OnDrawGizmosSelected()
    {
        if (spawnAreaCenter == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(spawnAreaCenter.position, spawnAreaSize);
    }
}
