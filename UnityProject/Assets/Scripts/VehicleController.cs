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
        [Range(-1, 1)] public float steeringFactor;
    }

    public float maxTorque;
    public float maxSteering;
    public Axle[] axles = {};

	public void EnableVechicle(){
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		//StartEngine ();
	}
		
	public float respawnTime = 2f;

	private Vector3 startPosition;
	private Quaternion startRotation;

	public void Start()
	{
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

	public void OrderVechicle(float vertical,float horizontal){
		float torque = maxTorque * vertical;
		float steering = maxSteering * horizontal;

		foreach (var axle in axles) {
			axle.left.steerAngle = steering * axle.steeringFactor;
			axle.right.steerAngle = steering * axle.steeringFactor;

			if (axle.applyTorque) {
				axle.left.motorTorque = torque;
				axle.right.motorTorque = torque;
			}
		}
	}

	public void ReplaceCar()
	{
		StartCoroutine (ReplacingCar());
	}

	private IEnumerator ReplacingCar()
	{
		Vector3 startReplacePosition = transform.position;
		Quaternion startReplaceRotation = transform.rotation;

		bool replacing = true;
		float curT = 0f;
		float curTime = 0f;
		while (replacing)
		{
			replacing = !(curT >= 1f);

			transform.position = Vector3.Lerp (startReplacePosition, startPosition, curT);
			transform.rotation = Quaternion.Slerp(startReplaceRotation, startRotation, curT);

			curTime += Time.deltaTime;
			curT = curTime / respawnTime;

			yield return new WaitForEndOfFrame();
		}
	}
}
