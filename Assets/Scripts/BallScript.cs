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

    public void Update()
    {
        if (ball = GameObject.FindGameObjectWithTag("ball"))
        {
            chevrons = GameObject.FindGameObjectWithTag("chevrons");
            chevrons.transform.eulerAngles = new Vector3(-90, 0, ARCamera.transform.eulerAngles.y - 180);
        }

    }
    public void HitBall ()
    {
        slider = Object.FindObjectOfType<Slider>();
        ball = GameObject.FindGameObjectWithTag("ball");
        hitForce = slider.value;
        Debug.Log("hitForce: " + hitForce);
        Vector3 hitAngle = ARCamera.transform.forward;
        Debug.Log("hitAngle: "+ hitAngle);
        Vector3 force = hitAngle * hitForce * maxForce;
        Debug.Log("ball: " + ball);
        ball.GetComponent<Rigidbody>().AddForce(force);
        Debug.Log("Force: " + force);
        GameObject.FindGameObjectWithTag("gameManager").GetComponent<ARHitTest>().AddStrokes();
    }
}
