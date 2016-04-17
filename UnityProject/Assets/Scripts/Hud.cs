using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

    public InputField inputField;
    public Button startClient;

    public void Start () {
        startClient.onClick.AddListener( () => {
            NetworkManager.singleton.networkAddress = inputField.text;
            NetworkManager.singleton.StartClient();
            Destroy( startClient.gameObject );
            Destroy( inputField.gameObject );
        } );
    }



}
