using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour {

    GameObject ball;
    public Camera ARCamera;
    float hitForce;
    //Slider slider;
    public int maxForce;
    GameObject chevrons;
    public float aimDisplayThreshold;
    Vector3 velocity;
    Text helpText;
    bool helpPlayer;
    string helpPhase;
    Text hitPowerText;

    // touch input
    public Vector2 startPos;
    public Vector2 direction;

    Text m_Text;
    string message;


    public void OnEnable()
    {
        ball = GameObject.FindGameObjectWithTag("ball");
        if (ball.GetComponent<Rigidbody>())
        {
            velocity = ball.GetComponent<Rigidbody>().velocity;
        }
        else
        {
            Debug.Log("No Rigidbody attached to ball: ");
        }
        chevrons = GameObject.FindGameObjectWithTag("chevrons");
        ARCamera = Camera.main;
        m_Text = GameObject.FindGameObjectWithTag("touch").GetComponent<Text>();
        helpText = GameObject.FindGameObjectWithTag("help").GetComponent<Text>();
        hitPowerText = GameObject.FindGameObjectWithTag("hitPower").GetComponent<Text>();
        helpPlayer = true;
        helpPhase = "aim";
    }

    public void Update()
    {
        // move the aiming chevrons with the ball
        chevrons.transform.eulerAngles = new Vector3(-90, 0, ARCamera.transform.eulerAngles.y - 180);

        // track touch input
        m_Text.text = "Touch: " + message + " in direction" + direction;

        // track a single touch as a direction control
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // handle finger movements based on touch phase
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // record initial touch position
                    startPos = touch.position;
                    message = "Begun ";
                    helpText.text = "";
                    chevrons.gameObject.SetActive(true);
                    break;

                case TouchPhase.Moved:
                    // determine direction by comparing position to startPos
                    direction = touch.position - startPos;
                    hitPowerText.text = "Hit Power: " + (direction.y / Screen.height);
                    Debug.Log("Hit Power: " + (direction.y / Screen.height));
                    message = "Moving ";
                    helpText.text = "Drag your finger up to set the swing power. \n Release to hit the ball...";
                    helpPhase = "release";
                    break;

                case TouchPhase.Ended:
                    // report that the touch has ended
                    message = "Ending ";
                    chevrons.gameObject.SetActive(false);
                    HitBall();
                    helpText.text = "";
                    hitPowerText.text = "";
                    helpPlayer = false;
                    break;
            }
        }

        if (helpPlayer)
        {
            switch (helpPhase) {
            	case "aim":
            		Vector3 difference = ball.transform.position - ARCamera.transform.position;
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

    public void FixedUpdate()
    { 
        // track the velocity of the ball
        if (velocity.x > 0 || velocity.y > 0 || velocity.z > 0)
        {
        	Debug.Log("Ball velocity: " + velocity);
        }
            
        // show or hide the chevrons based on the velocity of the ball
        //if (Mathf.Abs(velocity.x) <= aimDisplayThreshold && Mathf.Abs(velocity.z) <= aimDisplayThreshold)
        //{
        //    chevrons.gameObject.SetActive(true);
        //} 
        //else
        //{
        //    chevrons.gameObject.SetActive(false);
        //}         
    }

    public void HitBall ()
    {
        // define the game objects
        //ARCamera = Camera.main;
        //slider = Object.FindObjectOfType<Slider>();
        //ball = GameObject.FindGameObjectWithTag("ball");
        //chevrons = GameObject.FindGameObjectWithTag("chevrons");

        Vector3 hitAngle = ARCamera.transform.forward;
        //Debug.Log("hitAngle: "+ hitAngle);

        //hitForce = slider.value;
        hitForce = direction.y / Screen.height;
        //Debug.Log("hitForce: " + hitForce);

        Vector3 force = hitAngle * hitForce * maxForce;
        //Debug.Log("ball: " + ball);
        ball.GetComponent<Rigidbody>().AddForce(force);
        Debug.Log("Force: " + force);
        GameObject.FindGameObjectWithTag("gameManager").GetComponent<ARHitTest>().AddStrokes();
    }

    void HelpPlayer()
    {
        
    }
}
