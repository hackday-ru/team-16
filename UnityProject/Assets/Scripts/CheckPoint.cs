using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class CheckPoint : MonoBehaviour
{
    public bool isFinish;
    public CheckPoint next;

    public void SetActive(bool active)
    {
        transform.GetChild(0).gameObject.SetActive(active);
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        if (next == null) { return; }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, next.transform.position);
    }

#endif
}
