using UnityEngine;

public class BeadSpawner : MonoBehaviour
{
  [SerializeField] private GameObject tearBallPrefab;
    [SerializeField] private Transform spawnAreaTop; // 상단 범위(박스)
    [SerializeField] private Vector2 spawnBoxSize = new Vector2(8f, 0f);
    [SerializeField] private float spawnHeight = 6f;

    public GameObject SpawnBall(AudioClip clip)
    {
        if (!tearBallPrefab) return null;

        Vector3 pos = GetRandomTopPosition();
        var go = Instantiate(tearBallPrefab, pos, Quaternion.identity);
        var ball = go.GetComponent<TearBall>();
        if (ball) ball.BindAudio(clip);

        return go;
    }

    private Vector3 GetRandomTopPosition()
    {
        Vector3 center = spawnAreaTop ? spawnAreaTop.position : Vector3.zero;
        float x = Random.Range(-spawnBoxSize.x * 0.5f, spawnBoxSize.x * 0.5f);
        return new Vector3(center.x + x, center.y + spawnHeight, center.z);
    }
}
