using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bead : MonoBehaviour
{

    // 필요시 확장: OnCollisionEnter로 바닥 연출 등

    [SerializeField] private AudioSource audioSource; // 구슬 근처/충돌 시 사운드 연동 가능
    [SerializeField] private ParticleSystem onLandVfx;

    private bool _isLanded;

    public void BindAudio(AudioClip clip)
    {
        if (!audioSource) audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource && clip)
        {
            audioSource.clip = clip;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (_isLanded) return;
        _isLanded = true;

        if (onLandVfx) onLandVfx.Play();
        // TODO: 바닥에 안착 후 약한 끈적/감쇠 처리(Physics Material/Drag 조정)
    }
}
