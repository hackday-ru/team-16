using UnityEngine;
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

	private bool powered = false;

	public void SelectVechicle(){
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
	}

	public void StartEngine(){
		powered = true;
	}

    public void FixedUpdate()
    {
		if (powered) {
			float torque = maxTorque * Input.GetAxis ("Vertical");
			float steering = maxSteering * Input.GetAxis ("Horizontal");

			foreach (var axle in axles) {
				axle.left.steerAngle = steering * axle.steeringFactor;
				axle.right.steerAngle = steering * axle.steeringFactor;

				if (axle.applyTorque) {
					axle.left.motorTorque = torque;
					axle.right.motorTorque = torque;
				}
			}
		}
    }


}
