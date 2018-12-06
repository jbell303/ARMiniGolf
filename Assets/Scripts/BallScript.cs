using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    Rigidbody rb;
    public float hitForce;
    Camera ARCamera;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        ARCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            Vector3 hitAngle = ARCamera.transform.forward;
            Debug.Log("hit angle: " + hitAngle);
            rb.AddForce(hitAngle * hitForce);
        }
	}
}
