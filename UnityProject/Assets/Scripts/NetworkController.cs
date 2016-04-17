using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkManager {

	public NetworkBehaviour	serverSidePrefab;
	public NetworkBehaviour clientSidePrefab;

	private NetworkBehaviour networkLogic;

	private NetworkDiscovery networkDiscovery;
	bool lookForLANGames = false;
	float waitTime;

	// Use this for initialization
	void Start ()
	{
		networkDiscovery = GetComponentInParent<NetworkDiscovery> ();
		networkDiscovery.Initialize ();
	
		#if UNITY_IPHONE
		StopServer ();
		StopHost ();

		networkDiscovery.StartAsClient ();

		lookForLANGames = true;
		waitTime = 3;
		#endif
	}

	// Update is called once per frame
	void Update ()
	{
		#if !UNITY_IPHONE
		if (!networkLogic) {
			if (Input.GetButton ("Submit")) {
                Debug.Log("Starting server mode");
				StartHost();

				networkDiscovery.StartAsServer ();

				networkLogic = Instantiate (serverSidePrefab, Vector3.zero, Quaternion.identity) as NetworkBehaviour;
				networkLogic.transform.SetParent (gameObject.transform);
				(networkLogic as iNetBehaviour).Initialize ();
			} else if (Input.GetButton ("Jump")) {
                Debug.Log("Starting client mode");
				StopServer();
				StopHost();

				networkDiscovery.StartAsClient ();

				networkLogic = Instantiate (clientSidePrefab, Vector3.zero, Quaternion.identity) as NetworkBehaviour;
				networkLogic.transform.SetParent (gameObject.transform);
				(networkLogic as iNetBehaviour).Initialize ();
			}
		}
		#else
		if (lookForLANGames) {
			waitTime -= Time.deltaTime;
			if (waitTime < 0) {
				lookForLANGames = false;
				networkDiscovery.StopBroadcast ();
				networkDiscovery.StartAsServer ();

				StartHost ();
			}
		}
		#endif
	}

	#if !UNITY_IPHONE
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 400, 100), "Press Submit to start Server / Press Jump to start client");
	}
	#endif

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
