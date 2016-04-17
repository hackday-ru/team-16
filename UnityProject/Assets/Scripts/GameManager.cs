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
		if (NetworkClient.active) {
			EventSubmitSelect += SubmitSelect;
		}
	}

	void OnEnable(){
		//gameObject.transform.position = new Vector3 (spawnX, spawnY, spawnZ);
	}

	// Update is called once per frame
	void Update () {

	}

	public delegate void SubmitSelectEvent(int index);

	[SyncEvent]
	public event SubmitSelectEvent EventSubmitSelect;

	public void RunSubmitEvent(int index){
		EventSubmitSelect (index);
	}

	public void SubmitSelect(int index){
		var selectedCar = cars [index];
		cars.RemoveAt (index);
		selectedCar.transform.FindChild("vechicle").GetComponent<VehicleController> ().EnableVechicle ();
		selectedCar.transform.FindChild ("Platform").gameObject.SetActive (false);
	}

}
