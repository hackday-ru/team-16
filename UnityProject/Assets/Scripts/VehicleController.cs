using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

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

	private float chekingHoldTime = 2f;
	private Dictionary<KeyCode, float> checkingHoldButtons = new Dictionary<KeyCode, float>();

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

		CheckHoldButton (KeyCode.A);
    }

	private void CheckHoldButton(KeyCode keyCode)
	{
		float curValue;
		if (Input.GetKeyDown (keyCode) && !checkingHoldButtons.TryGetValue(keyCode, out curValue))
		{
			Debug.LogWarning ("CheckHoldButton GetKeyDown");
			checkingHoldButtons.Add (keyCode, 0f);
		}

		if (Input.GetKeyUp (keyCode) && checkingHoldButtons.TryGetValue(keyCode, out curValue))
		{
			Debug.LogWarning ("CheckHoldButton GetKeyUp");
			checkingHoldButtons.Remove (keyCode);
		}

		if (checkingHoldButtons.TryGetValue(keyCode, out curValue))
		{
			checkingHoldButtons[keyCode] += Time.deltaTime;
			if (checkingHoldButtons[keyCode] >= chekingHoldTime)
			{
				Debug.LogWarning ("CheckHoldButton Done");
				checkingHoldButtons.Remove (keyCode);
			}
		}
	}
}
