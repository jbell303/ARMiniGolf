using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;

public class ARHitTest : MonoBehaviour {
	public Camera ARCamera; //the Virtual Camera used for AR
	public GameObject hitPrefab; //prefab we place on a hit test
    public Button spawnButton;
    public Button restartButton;
    public Button hitButton;

    private List<GameObject> spawnedObjects = new List<GameObject>(); //array used to keep track of spawned objects

	/// <summary>
	/// Function that is called on 
	/// NOTE: HIT TESTS DON'T WORK IN ARKIT REMOTE
	/// </summary>
	public void SpawnHitObject() {
		ARPoint point = new ARPoint { 
			x = 0.5f, //do a hit test at the center of the screen
			y = 0.5f
		};

		// prioritize result types
		ARHitTestResultType[] resultTypes = {
			//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, // if you want to use bounded planes
			//ARHitTestResultType.ARHitTestResultTypeExistingPlane, // if you want to use infinite planes 
			ARHitTestResultType.ARHitTestResultTypeFeaturePoint // if you want to hit test on feature points
		}; 

		foreach (ARHitTestResultType resultType in resultTypes) {
			if (HitTestWithResultType (point, resultType)) {
				return;
			}
		}
	}

	bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes) {
		List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
		if (hitResults.Count > 0) {
			foreach (var hitResult in hitResults) {
                //TODO: get the position and rotations to spawn the hat
                Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform); //returns a Vector3 in Unity Coordinates
                Quaternion rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform); //returns a Quaternion in Unity Coordinates

                // add hat to the list of spawned objects, instantiate a new hat
                spawnedObjects.Add(Instantiate(hitPrefab, position, rotation));
                spawnButton.enabled = false;
                hitButton.enabled = true;
                return true;
			}
		}
		return false;
	}

	// Fixed Update is called once per frame
	void FixedUpdate () {
		if (Input.GetMouseButtonDown(0)) { //this works with touch as well as with a mouse
            //RemoveObject (Input.mousePosition); 
		}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            Debug.Log("ball exited play area...");
            restartButton.enabled = true;
        }
    }

    public void removeCourse()
    {
        if (spawnedObjects.Remove(spawnedObjects[0]))
        {
            Destroy(spawnedObjects[0]);
        }
    }

    public void ResetGame()
    {
        removeCourse();
        spawnButton.enabled = true;
        restartButton.enabled = false;
    }

    public void RemoveObject(Vector2 point) {
		RaycastHit hit;
		if (Physics.Raycast (ARCamera.ScreenPointToRay (point), out hit)) {
			GameObject item = hit.collider.transform.parent.gameObject;
			if (spawnedObjects.Remove (item)) {
				Destroy(item);
			}
		}
	}
}