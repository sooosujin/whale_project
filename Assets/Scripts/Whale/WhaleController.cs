using UnityEngine;

public class WhaleController : MonoBehaviour
{
    public string animationStateName = "Swim"; // Animator 안의 상태 이름
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("WhaleController: Animator component not found!");
        }
    }

    void Start()
    {
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        if (animator == null) return;

        animator.Play(animationStateName);
    }

    // 필요하면 나중에 다른 애니메이션으로 바꾸기 위한 함수
    public void PlayAnimationByName(string stateName)
    {
        if (animator == null) return;

        animationStateName = stateName;
        animator.Play(animationStateName);
    }
}
