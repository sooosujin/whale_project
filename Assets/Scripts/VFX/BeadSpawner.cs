using UnityEngine;

public class BeadSpawner : MonoBehaviour
{
    public GameObject beadPrefab;
    public int spawnOnStart = 0; // 테스트용
    public float radius = 8f;
    public float sinkSpeed = 0.5f;

    public static int TotalBeads
    {
        get;
        private set;
    }

    public void SpawnBead()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * radius;
        pos.y = Random.Range(2f, 6f);
        var go = Instantiate(beadPrefab, pos, Quaternion.identity);
        go.AddComponent<BeadSink>().speed = sinkSpeed;
        TotalBeads++;
    }

    // 외부에서 CSV 레코드 수만큼 호출하면 됨
    public void SpawnFromCSVCount(int count)
    {
        for (int i = 0; i < count; i++) SpawnBead();
    }

}

public class BeadSink : MonoBehaviour
{
    public float speed = 0.5f;
    float floorY = -2f;
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (transform.position.y < floorY) // 바닥 도달하면 약간 흔들림만 유지하거나 멈춤
        {
            var pos = transform.position; pos.y = floorY;
            transform.position = pos;
            enabled = false;
        }
    }
}
