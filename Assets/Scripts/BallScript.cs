using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour {

    GameObject ball;
    public Camera ARCamera;
    float hitForce;
    Slider slider;
    public int maxForce;

    private void Start()
    {
        slider = Object.FindObjectOfType<Slider>();
        hitForce = 0;
    }
    public void HitBall ()
    {
    	ball = GameObject.FindGameObjectWithTag("ball");
        hitForce = slider.value;
        Debug.Log("hitForce: " + hitForce);
        Vector3 hitAngle = ARCamera.transform.forward;
        Debug.Log("hitAngle: "+ hitAngle);
        Vector3 force = hitAngle * hitForce * maxForce;
        Debug.Log("ball: " + ball);
        ball.GetComponent<Rigidbody>().AddForce(force);
        Debug.Log("Force: " + force);
    }
}
