﻿using UnityEngine;
using System;

public class VehicleController : MonoBehaviour
{
    [Serializable]
    public class Axle
    {
        public WheelCollider left;
        public WheelCollider right;
        public bool applyTorque;
        [Range(-1, 1)] public float steeringFactor;
    }

    public float maxTorque;
    public float maxSteering;
    public Axle[] axles = {};

    public void FixedUpdate()
    {
        float torque = maxTorque * Input.GetAxis("Vertical");
        float steering = maxSteering * Input.GetAxis("Horizontal");

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
    }


}
