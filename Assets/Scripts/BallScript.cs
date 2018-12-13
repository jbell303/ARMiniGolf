using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour {

    // define the game objects
    GameObject ball;
    GameObject aimingChevrons;
    public Camera ARCamera;

    public int maxForce;
    Text hitPowerText;

    bool isHelpEnabled;
    string helpPhase;
    Text helpText;


    // touch input
    public Vector2 startPos;
    public Vector2 direction;


    public void OnEnable()
    {
        ball = GameObject.FindGameObjectWithTag("ball");
        ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        aimingChevrons = GameObject.FindGameObjectWithTag("chevrons");
        ARCamera = Camera.main;
        helpText = GameObject.FindGameObjectWithTag("help").GetComponent<Text>();
        hitPowerText = GameObject.FindGameObjectWithTag("hitPower").GetComponent<Text>();
        isHelpEnabled = true;
        helpPhase = "aim";
    }

    public void Update()
    {
        // track touch input
        // track a single touch as swing power control
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // handle finger movements based on touch phase
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // record initial touch position and display the aiming chevrons
                    startPos = touch.position;
                    helpText.text = "";
                    aimingChevrons.gameObject.SetActive(true);
                    break;

                case TouchPhase.Moved:
                    // determine direction of touch movement by comparing position to startPos
                    //direction = touch.position - startPos;
                    direction = startPos - touch.position;

                    // hit power is the amount of touch movement in relation to the height of the 
                    // screen. Displayed as percent rounded to one decimal place (e.g. 50.0)
                    hitPowerText.text = "Hit Power: " + (direction.y / Screen.height * 100).ToString("F1");
                    if (isHelpEnabled)
                    {
                        helpText.text = "Drag your finger up to set the swing power. \n Release to hit the ball...";
                    }
                    helpPhase = "release"; // placeholder phase, prevents swing phase text from being displayed
                    break;

                case TouchPhase.Ended:
                    // hide the aiming chevrons and hit the ball
                    aimingChevrons.gameObject.SetActive(false);
                    HitBall();
                    helpText.text = "";
                    hitPowerText.text = "";
                    isHelpEnabled = false;
                    break;
            }
        }

        // move the aiming chevrons with the camera
            aimingChevrons.transform.eulerAngles = new Vector3(-90, 0, ARCamera.transform.eulerAngles.y - 180);

        // if help is enabled, text is displayed to guide the player through the swing process
        if (isHelpEnabled)
        {
            switch (helpPhase) {
            	case "aim":
                    // compare the player's position relative to the ball
            		Vector3 difference = ball.transform.position - ARCamera.transform.position;
                    
                    // difference in x axis is negative if the player is in front of the ball
                    if (difference.x < 0)
            		{
                		helpText.text = "Stand behind the ball...";
            		} else {
            			helpPhase = "swing";
            		}
            		break;
            	
            	case "swing":
            		helpText.text = "Tap and hold to begin the swing...";
            		break;
            }
        }
    }

    public void HitBall ()
    {
        // define the hit angle based on where the camera is pointing
        Vector3 hitAngle = ARCamera.transform.forward;

        // define the force of the hit based on touch position
        float hitForce = direction.y / Screen.height;

        // hit vector = angle * percent of max force based on touch position
        Vector3 force = hitAngle * hitForce * maxForce;
        ball.GetComponent<Rigidbody>().AddForce(force);

        // add stroke to total stroke count
        GameObject.FindGameObjectWithTag("gameManager").GetComponent<ARHitTest>().AddStrokes();
    }
}
