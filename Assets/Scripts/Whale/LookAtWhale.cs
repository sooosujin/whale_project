using UnityEngine;

public class LookAtWhale : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target == null) return;

        transform.LookAt(target.position);
    }
}
