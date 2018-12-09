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
    GameObject chevrons;
    public float aimDisplayThreshold;

    public void Awake()
    {
    	ball = GameObject.FindGameObjectWithTag("ball");
    	chevrons = GameObject.FindGameObjectWithTag("chevrons");
    	ARCamera = Camera.main;
    }

    public void Update()
    {
        // move the aiming chevrons with the ball
        chevrons.transform.eulerAngles = new Vector3(-90, 0, ARCamera.transform.eulerAngles.y - 180);   
    }

    public void FixedUpdate()
    { 
        // track the velocity of the ball
        Vector3 velocity = ball.GetComponent<Rigidbody>().velocity;
        if (velocity.x > 0 || velocity.y > 0 || velocity.z > 0)
        {
        	Debug.Log("Ball velocity: " + velocity);
        }
            
        // show or hide the chevrons based on the velocity of the ball
        if (Mathf.Abs(velocity.x) <= aimDisplayThreshold && Mathf.Abs(velocity.z) <= aimDisplayThreshold)
        {
            chevrons.gameObject.SetActive(true);
        } 
        else
        {
            chevrons.gameObject.SetActive(false);
        }         
    }

    public void HitBall ()
    {
    	ARCamera = Camera.main;
        slider = Object.FindObjectOfType<Slider>();
        ball = GameObject.FindGameObjectWithTag("ball");
        chevrons = GameObject.FindGameObjectWithTag("chevrons");
        hitForce = slider.value;
        //Debug.Log("hitForce: " + hitForce);
        Vector3 hitAngle = ARCamera.transform.forward;
        //Debug.Log("hitAngle: "+ hitAngle);
        Vector3 force = hitAngle * hitForce * maxForce;
        //Debug.Log("ball: " + ball);
        ball.GetComponent<Rigidbody>().AddForce(force);
        Debug.Log("Force: " + force);
        GameObject.FindGameObjectWithTag("gameManager").GetComponent<ARHitTest>().AddStrokes();
    }
}
