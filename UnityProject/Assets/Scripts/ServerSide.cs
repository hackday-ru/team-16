using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerSide : NetworkBehaviour, iNetBehaviour {

	public NetworkDiscovery networkDiscovery;
	//private NetworkManager netManager;

	// Use this for initialization
	public void Initialize ()
	{
		networkDiscovery = GetComponentInParent<NetworkDiscovery> ();
		networkDiscovery.Initialize ();
		networkDiscovery.StartAsServer ();
		Debug.Log ("**** Server started ****");

		//netManager = NetworkManager.singleton;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	[Command]
	void CmdClientReady()
	{

	}

//	void OnGUI() {
//		GUI.Label(new Rect(100, 100, 100, 100), "Server started");
//	}
}
