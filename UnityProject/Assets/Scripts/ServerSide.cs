using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerSide : NetworkBehaviour, iNetBehaviour {

	// Use this for initialization
	public void Initialize ()
	{
		Debug.Log ("**** Server started ****");

		//netManager = NetworkManager.singleton;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

//	void OnGUI() {
//		GUI.Label(new Rect(100, 100, 100, 100), "Server started");
//	}
}
