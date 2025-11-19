using UnityEngine;

public class WhaleController : MonoBehaviour
{
    public string animationStateName = "Swim";
    Animator animator;
    void Awake() { animator = GetComponent<Animator>(); }
    void Start() { animator.Play(animationStateName); }
}
