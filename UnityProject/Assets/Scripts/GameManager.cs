using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour {

	//[SyncVar]
	public List<GameObject> cars;

//	public float spawnX = 0.0f;
//	public float spawnY = 0.0f;
//	public float spawnZ = 0.0f;

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable(){
		//gameObject.transform.position = new Vector3 (spawnX, spawnY, spawnZ);
	}

	// Update is called once per frame
	void Update () {

	}
		
	/*private void CheckHoldButton(KeyCode keyCode, Action callback)
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
	}*/

	[ClientRpc]
	public void RpcSubmitSelect(int index){
		var selectedCar = cars [index];
		cars.RemoveAt (index);
		selectedCar.transform.FindChild("vechicle").GetComponent<VehicleController> ().EnableVechicle ();
		selectedCar.transform.FindChild ("Platform").gameObject.SetActive (false);
	}

}
