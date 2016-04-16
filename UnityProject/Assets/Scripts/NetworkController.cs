using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkController : NetworkManager {

	public NetworkBehaviour	serverSidePrefab;
	public NetworkBehaviour clientSidePrefab;

	private NetworkBehaviour networkLogic;

	private NetworkManager networkManager;

	// Use this for initialization
	void Start ()
	{
		networkManager = GetComponent<NetworkManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!networkLogic) {
			if (Input.GetButton ("Submit")) {
				StartHost ();

				networkLogic = Instantiate (serverSidePrefab, Vector3.zero, Quaternion.identity) as NetworkBehaviour;
				networkLogic.transform.SetParent (gameObject.transform);
				(networkLogic as iNetBehaviour).Initialize ();
			} else if (Input.GetButton ("Jump")) {
				StopServer ();
				OnStopHost ();

				networkLogic = Instantiate (clientSidePrefab, Vector3.zero, Quaternion.identity) as NetworkBehaviour;
				networkLogic.transform.SetParent (gameObject.transform);
				(networkLogic as iNetBehaviour).Initialize ();
			}
		}
	}

	public override void OnStartClient(NetworkClient client)
	{
		Debug.Log ("Start client" + client);
	}

	public override void OnStopClient()
	{
		Debug.Log ("Stop client");
	}

	public override void OnStartHost()
	{
		Debug.Log ("Start host");
	}

	public override void OnStopHost ()
	{
		Debug.Log("Stop host");
	}

	public override void OnStartServer()
	{
		Debug.Log ("Start server");
	}

	public override void OnStopServer ()
	{
		Debug.Log ("Stop server");
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log ("Client connected: " + conn.hostId);
	}
}
