using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LANDiscoverController : NetworkDiscovery {
	public override void OnReceivedBroadcast (string fromAddress, string data)
	{
		//StopBroadcast ();

		Debug.Log ("Connecting: " + fromAddress);
		NetworkManager.singleton.StopHost();
		NetworkManager.singleton.networkAddress = fromAddress;

		Destroy (this.gameObject);
	}

	void OnDestroy()
	{
		NetworkManager.singleton.StartClient();
	}
}
