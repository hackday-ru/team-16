using UnityEngine;
using System.Collections;

public class WheelVisual : MonoBehaviour
{
    public WheelCollider wheelCollider;

    void LateUpdate()
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);

        transform.position = position;
        transform.rotation = rotation;
    }
}
