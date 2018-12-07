using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseScript : MonoBehaviour {
    GameObject gameManager;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            Debug.Log("ball exited play area...");
            gameManager = GameObject.FindGameObjectWithTag("gameManager");
            gameManager.GetComponent<ARHitTest>().SetOutOfBounds();
        }
    }
}
