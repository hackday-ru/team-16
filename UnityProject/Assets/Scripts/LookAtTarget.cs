using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        transform.LookAt(target);
    }
}
