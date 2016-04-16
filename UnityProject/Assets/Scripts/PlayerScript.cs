using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

	public GameManager gameManager;

	private bool carSelected=false;
	private int curSelected = 0;

	public GameObject carSelector;

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable(){
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		carSelector = gameManager.transform.FindChild ("Selector").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		float axis = Input.GetAxis ("Horizontal");
		int direction = axis > 0 ? 1 : axis < 0 ? -1 : 0;
		curSelected += direction;
		curSelected = Mathf.Clamp (curSelected, 0, gameManager.cars.Count-1);

		carSelector.transform.position = gameManager.cars [curSelected].transform.position;

		if (Input.GetKey ("return"))
			CmdSelectCar (curSelected);
	}

	[Command]
	void CmdSelectCar(int index){
		if (!carSelected && index >= 0 && index < gameManager.cars.Count) {
			carSelected = true;
			carSelector.SetActive (false);
			gameManager.cars[index].transform.FindChild("vechicle").GetComponent<VehicleController> ().StartEngine ();
			gameManager.RpcSubmitSelect (index);
		}
	}
}
