using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class VehicleController :NetworkBehaviour {
    [Serializable]
    public class Axle {
        public WheelCollider left;
        public WheelCollider right;
        public bool applyTorque;
        [Range( -1, 1 )]
        public float steeringFactor;
    }

    public float maxTorque;
    public float maxSteering;
    public float distanceToGround;
    public float jumpForce;
    public Axle [] axles = { };

    private float chekingHoldTime = 2f;
    private Dictionary<string, float> checkingHoldButtons = new Dictionary<string, float>();

    Rigidbody body;
    CheckPoint checkPoint;

    public void EnableVechicle () {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //StartEngine ();
    }

    public float respawnTime = 2f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public void Start () {
        body = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void OrderVechicle ( float vertical, float horizontal, float jump ) {
        float torque = maxTorque * vertical;
        float steering = maxSteering * horizontal;

        foreach ( var axle in axles ) {
            axle.left.steerAngle = steering * axle.steeringFactor;
            axle.right.steerAngle = steering * axle.steeringFactor;

            if ( axle.applyTorque ) {
                axle.left.motorTorque = torque;
                axle.right.motorTorque = torque;
            }
        }

        if ( jump > 0.5f ) {
            bool isGrounded = Physics.Raycast( transform.position, Vector3.down, distanceToGround );

            if ( isGrounded ) {
                body.AddForce( 0.0f, jumpForce, 0.0f, ForceMode.Impulse );
            }
        }
    }

    public void ReplaceCar () {

        if ( !isLocalPlayer )
            return;

        if ( transform.GetComponent<Rigidbody>().isKinematic ) {
            return;
        }

        StartCoroutine( ReplacingCar() );
    }

    private IEnumerator ReplacingCar () {
        transform.GetComponent<Rigidbody>().isKinematic = true;

        Vector3 startReplacePosition = transform.position;
        Quaternion startReplaceRotation = transform.rotation;

        bool replacing = true;
        float curT = 0f;
        float curTime = 0f;
        while ( replacing ) {
            replacing = !( curT >= 1f );

            transform.position = Vector3.Lerp( startReplacePosition, startPosition, curT );
            transform.rotation = Quaternion.Slerp( startReplaceRotation, startRotation, curT );

            curTime += Time.deltaTime;
            curT = curTime / respawnTime;

            yield return new WaitForEndOfFrame();
        }

        transform.GetComponent<Rigidbody>().isKinematic = false;
    }

    void OnTriggerEnter ( Collider collider ) {
        if ( !isLocalPlayer )
            return;

        Debug.Log( "OnTriggerEnter" );

        var point = collider.GetComponent<CheckPoint>();
        if ( point == null || point == checkPoint ) {
            return;
        }

        if ( checkPoint == null || checkPoint.next == point ) {
            if ( checkPoint != null ) {
                checkPoint.next.SetActive( false );
            }

            checkPoint = point;
            checkPoint.next.SetActive( true );
        }
    }


    // Update is called once per frame
    void FixedUpdate () {
        if ( !isLocalPlayer ) {
            return;
        }



            float v = Input.GetAxis( "Vertical" );
            float h = Input.GetAxis( "Horizontal" );
            float j = Input.GetAxis( "Jump" );
            OrderVechicle( v, h, j );
            //CheckHoldButton (KeyCode.RightAlt, ReplaceCar);
            //CheckHoldButton (KeyCode.Joystick1Button1, ReplaceCar);

            if ( Input.GetButtonDown( "Submit" ) ) {
                ReplaceCar();
            }

        CheckHoldButton( "Submit", ReplaceCar );
    }

    private void CheckHoldButton ( string keyCode, Action callback ) {
        float curValue;
        if ( Input.GetButtonDown( keyCode ) && !checkingHoldButtons.TryGetValue( keyCode, out curValue ) ) {
            checkingHoldButtons.Add( keyCode, 0f );
        }

        if ( Input.GetButtonUp( keyCode ) && checkingHoldButtons.TryGetValue( keyCode, out curValue ) ) {
            checkingHoldButtons.Remove( keyCode );
        }

        if ( checkingHoldButtons.TryGetValue( keyCode, out curValue ) ) {
            checkingHoldButtons [keyCode] += Time.deltaTime;
            if ( checkingHoldButtons [keyCode] >= chekingHoldTime ) {
                checkingHoldButtons.Remove( keyCode );
                if ( callback != null ) {
                    callback();
                }
            }
        }
    }
}
