using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerScript : NetworkBehaviour {

	public GameManager gameManager;

	private bool carSelected=false;
	private int curSelected = 0;

	public GameObject carSelector;
	private bool powered = false;
	private VehicleController vechicle;

	private float chekingHoldTime = 2f;
	private Dictionary<KeyCode, float> checkingHoldButtons = new Dictionary<KeyCode, float>();

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable(){
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		carSelector = gameManager.transform.FindChild ("Selector").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if ( !isLocalPlayer ) {
			return;
		}

		//menu control
		float axis = Input.GetAxis ("Horizontal");
		int direction = axis > 0 ? 1 : axis < 0 ? -1 : 0;
		curSelected += direction;
		curSelected = Mathf.Clamp (curSelected, 0, gameManager.cars.Count-1);
		carSelector.transform.position = gameManager.cars [curSelected].transform.position;
		if (Input.GetKey ("return"))
			CmdSelectCar (curSelected);

		//car control
		if (powered) {

			float v = Input.GetAxis ("Vertical");
			float h = Input.GetAxis ("Horizontal");
			vechicle.OrderVechicle(v,h);

			CheckHoldButton (KeyCode.A, ReplaceCar);
		}
	}

	private void ReplaceCar(){
		vechicle.ReplaceCar ();
	}
		
	private void CheckHoldButton(KeyCode keyCode, Action callback)
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
	}

	[Command]
	void CmdSelectCar(int index){
		if (!carSelected && index >= 0 && index < gameManager.cars.Count) {
			carSelected = true;
			carSelector.SetActive (false);
			vechicle = gameManager.cars [index].transform.FindChild ("vechicle").GetComponent<VehicleController> ();
			powered = true;
			gameManager.RpcSubmitSelect (index);
		}
	}
}
