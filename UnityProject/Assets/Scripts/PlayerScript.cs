using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerScript : NetworkBehaviour
{

    public GameManager gameManager;

    private bool carSelected = false;
    private int curSelected = 0;

    public GameObject carSelector;
    private bool powered = false;

    private VehicleController vechicle;

    private float chekingHoldTime = 2f;
    private Dictionary<KeyCode, float> checkingHoldButtons = new Dictionary<KeyCode, float>();

    // Use this for initialization
    void Start ()
    {
//		if (NetworkClient.active) {
//			EventSelectCar += DoSelectCar;
//		}
    }

    void OnEnable()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        carSelector = gameManager.transform.FindChild("Selector").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if ( !isLocalPlayer ) {
            return;
        }

        //car control
		if (powered) {
			float v = Input.GetAxis ("Vertical");
			float h = Input.GetAxis ("Horizontal");
			float j = Input.GetAxis ("Jump");
			vechicle.OrderVechicle (v, h, j);
			CheckHoldButton (KeyCode.RightAlt, ReplaceCar);
			CheckHoldButton (KeyCode.Joystick1Button1, ReplaceCar);
		} else {
			//menu control
			float axis = Input.GetAxis("Horizontal");
			int direction = axis > 0 ? 1 : axis < 0 ? -1 : 0;
			curSelected += direction;
			curSelected = Mathf.Clamp(curSelected, 0, gameManager.cars.Count - 1);
			carSelector.transform.position = gameManager.cars[curSelected].transform.position;
			if (Input.GetKey("return"))
				CmdSelectCar(curSelected);
		}
    }

    private void ReplaceCar()
    {
        vechicle.ReplaceCar();
    }

    private void CheckHoldButton(KeyCode keyCode, Action callback)
    {
        float curValue;
        if (Input.GetKeyDown(keyCode) && !checkingHoldButtons.TryGetValue(keyCode, out curValue))
        {
            checkingHoldButtons.Add(keyCode, 0f);
        }

        if (Input.GetKeyUp(keyCode) && checkingHoldButtons.TryGetValue(keyCode, out curValue))
        {
            checkingHoldButtons.Remove(keyCode);
        }

        if (checkingHoldButtons.TryGetValue(keyCode, out curValue))
        {
            checkingHoldButtons[keyCode] += Time.deltaTime;
            if (checkingHoldButtons[keyCode] >= chekingHoldTime)
            {
                checkingHoldButtons.Remove(keyCode);
                if (callback != null)
                {
                    callback();
                }
            }
        }
    }

    [Command]
    void CmdSelectCar(int index)
    {
        if (!carSelected && index >= 0 && index < gameManager.cars.Count) {
            carSelected = true;
			RpcSelectCar (index);
            //powered = true;
			gameManager.RunSubmitEvent (index);
        }
    }

	//public delegate void SelectCarEvent(int index);

	//[SyncEvent]
	//public event SelectCarEvent EventSelectCar;

	[ClientRpc]
	void RpcSelectCar(int index){
		if (isLocalPlayer) {
			carSelector.SetActive(false);
			powered = true;
			vechicle = gameManager.cars [index].transform.FindChild ("vechicle").GetComponent<VehicleController> ();
			//Camera.main.GetComponent<LookAtTarget> ().target = vechicle.transform;
		}
	}
}
