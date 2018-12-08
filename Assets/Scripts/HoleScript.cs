using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("tracked a collision...");
        GameObject gameManager = GameObject.FindGameObjectWithTag("gameManager");
        gameManager.GetComponent<ARHitTest>().WinGame();
    }
}
