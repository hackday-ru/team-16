using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class VehicleController : NetworkBehaviour
{
    [Serializable]
    public class Axle
    {
        public WheelCollider left;
        public WheelCollider right;
        public bool applyTorque;
        [Range(-1, 1)]
        public float steeringFactor;
    }

    public float maxTorque;
    public float maxSteering;
    public float distanceToGround;
    public float jumpForce;
    public Axle[] axles = {};

    Rigidbody body;
    CheckPoint checkPoint;

    public void EnableVechicle()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //StartEngine ();
    }

    public float respawnTime = 2f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void OrderVechicle(float vertical, float horizontal, float jump)
    {
        float torque = maxTorque * vertical;
        float steering = maxSteering * horizontal;

        foreach (var axle in axles)
        {
            axle.left.steerAngle = steering * axle.steeringFactor;
            axle.right.steerAngle = steering * axle.steeringFactor;

            if (axle.applyTorque)
            {
                axle.left.motorTorque = torque;
                axle.right.motorTorque = torque;
            }
        }

        if (jump > 0.5f)
        {
            bool isGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround);

            if (isGrounded)
            {
                body.AddForce(0.0f, jumpForce, 0.0f, ForceMode.Impulse);
            }
        }
    }

    public void ReplaceCar()
    {
        StartCoroutine(ReplacingCar());
    }

    private IEnumerator ReplacingCar()
    {
        Vector3 startReplacePosition = transform.position;
        Quaternion startReplaceRotation = transform.rotation;

        bool replacing = true;
        float curT = 0f;
        float curTime = 0f;
        while (replacing) {
            replacing = !(curT >= 1f);

            transform.position = Vector3.Lerp(startReplacePosition, startPosition, curT);
            transform.rotation = Quaternion.Slerp(startReplaceRotation, startRotation, curT);

            curTime += Time.deltaTime;
            curT = curTime / respawnTime;

            yield return new WaitForEndOfFrame();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter");
        var point = collider.GetComponent<CheckPoint>();
        if (point == null || point == checkPoint) { return; }

        if (checkPoint == null || checkPoint.next == point)
        {
            if (checkPoint != null) { checkPoint.next.SetActive(false); }

            checkPoint = point;
            checkPoint.next.SetActive(true);
        }
    }
}
