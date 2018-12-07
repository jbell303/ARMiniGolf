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
        ball = GameObject.FindGameObjectWithTag("ball");
        slider = Object.FindObjectOfType<Slider>();
        hitForce = 0;
    }
    public void HitBall ()
    {
        
        hitForce = slider.value;
        Vector3 hitAngle = ARCamera.transform.forward;
        Vector3 force = hitAngle * hitForce * maxForce;
        ball.GetComponent<Rigidbody>().AddForce(force);
        Debug.Log("Force: " + force);
    }
}
