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

	private bool powered = false;

	public void EnableVechicle(){
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		//StartEngine ();
	}

	public void StartEngine(){
		powered = true;
	}
		
	public float respawnTime = 2f;
	private float chekingHoldTime = 2f;
	private Dictionary<KeyCode, float> checkingHoldButtons = new Dictionary<KeyCode, float>();

	private Vector3 startPosition;
	private Quaternion startRotation;

	public void Start()
	{
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

    public void Update ()
    {
        if ( !isLocalPlayer ) {
            return;
        }

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

		CheckHoldButton (KeyCode.A, ReplaceCar);
    }

	private void CheckHoldButton(KeyCode keyCode, Action callback)
	{
		float curValue;
		if (Input.GetKeyDown (keyCode) && !checkingHoldButtons.TryGetValue(keyCode, out curValue))
		{
			checkingHoldButtons.Add (keyCode, 0f);
		}

		if (Input.GetKeyUp (keyCode) && checkingHoldButtons.TryGetValue(keyCode, out curValue))
		{
			checkingHoldButtons.Remove (keyCode);
		}

		if (checkingHoldButtons.TryGetValue(keyCode, out curValue))
		{
			checkingHoldButtons[keyCode] += Time.deltaTime;
			if (checkingHoldButtons[keyCode] >= chekingHoldTime)
			{
				checkingHoldButtons.Remove (keyCode);
				if (callback != null)
				{
					callback ();
				}
			}
		}
	}

	private void ReplaceCar()
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
