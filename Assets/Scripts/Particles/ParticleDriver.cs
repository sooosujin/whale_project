using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDriver : MonoBehaviour
{
   [Header("CSV")]
    [SerializeField] private string csvFileName = "audio_test_0_f0.csv"; // StreamingAssets 안
    [SerializeField] private float timeScale = 1f;  // 1=실시간 재생
    [SerializeField] private Vector2 f0RangeHz = new Vector2(50f, 300f); // f0 정규화 범위

    [Header("Mapping (쉬운 느낌값)")]
    [SerializeField] private float simSpeedMin = 0.6f;
    [SerializeField] private float simSpeedMax = 2.0f;
    [SerializeField] private float noiseMin = 0.05f;
    [SerializeField] private float noiseMax = 0.8f;
    [SerializeField] private float orbitalYMin = -0.2f;
    [SerializeField] private float orbitalYMax = 0.8f;

    [Header("Energy")]
    [SerializeField] private bool useValue01AsEnergy = true; // CSV 에너지 감(0~1) 사용
    [SerializeField] private float energyBoost = 1.0f;

    private CsvF0Track _track;
    private ParticleSystem _ps;
    private ParticleSystem.NoiseModule _noise;
    private ParticleSystem.VelocityOverLifetimeModule _vel;
    private float _t;

    void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _noise = _ps.noise;
        _vel = _ps.velocityOverLifetime;
        _track = CsvF0Track.LoadFromStreamingAssets(csvFileName);
        if (_track == null) enabled = false;
    }

    void OnEnable() { _t = 0f; }

    void Update()
    {
        if (_track == null || _track.samples.Count == 0) return;

        _t += Time.deltaTime * timeScale;
        if (_t > _track.Duration) _t -= _track.Duration; // 루프

        var s = _track.SampleAt(_t);

        // f0(Hz) → 0..1
        float nf0 = Mathf.InverseLerp(f0RangeHz.x, f0RangeHz.y, Mathf.Max(0.0001f, s.f0SmoothHz));
        nf0 = Mathf.Clamp01(nf0);

        // 에너지(무성 구간이면 0)
        float energy = useValue01AsEnergy ? (s.voiced ? s.value01 * energyBoost : 0f) : 1f;

        // 1) 시뮬레이션 속도
        var main = _ps.main;
        main.simulationSpeed = Mathf.Lerp(simSpeedMin, simSpeedMax, nf0);

        // 2) 노이즈 강도/흐름
        _noise.enabled = true;
        _noise.strength = Mathf.Lerp(noiseMin, noiseMax, nf0) * energy;
        _noise.frequency = Mathf.Lerp(0.2f, 1.5f, nf0);
        _noise.scrollSpeed = Mathf.Lerp(0.1f, 1.5f, nf0);

        // 3) 부드러운 원운동 느낌(세로 공전)
        _vel.enabled = true;
        _vel.orbitalY = Mathf.Lerp(orbitalYMin, orbitalYMax, nf0) * energy;

        // 무성 구간: 움직임 최소화하면 대비 ↑
        if (!s.voiced)
        {
            _noise.strength = 0f;
            _vel.orbitalY = 0f;
        }
    }
}
