using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientSide : NetworkBehaviour, iNetBehaviour {

	// Use this for initialization
	public void Initialize () {
		Debug.Log ("**** Client started ****");
	}

	// Update is called once per frame
	void Update () {

	}

//	void OnGUI() {
//		GUI.Label(new Rect(100, 100, 100, 100), "Client started");
//	}
}
