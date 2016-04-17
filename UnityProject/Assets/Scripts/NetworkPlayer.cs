using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

    public MeshRenderer body;
    public Material newMaterial;

    public override void OnStartLocalPlayer () {
        Camera.main.GetComponent<LookAtTarget>().target = transform;
        //body.sharedMaterial = newMaterial;
        //body.sharedMaterial.color = Color.red;
    }

}
