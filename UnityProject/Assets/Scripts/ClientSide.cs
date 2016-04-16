using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientSide : NetworkBehaviour, iNetBehaviour {

	private NetworkDiscovery networkDiscovery;

	// Use this for initialization
	public void Initialize () {
		networkDiscovery = GetComponentInParent<NetworkDiscovery> ();
		networkDiscovery.Initialize ();
		networkDiscovery.StartAsClient ();
		Debug.Log ("**** Client started ****");
	}

	// Update is called once per frame
	void Update () {

	}

//	void OnGUI() {
//		GUI.Label(new Rect(100, 100, 100, 100), "Client started");
//	}
}
