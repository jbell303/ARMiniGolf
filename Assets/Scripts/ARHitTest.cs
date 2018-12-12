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
    public Text strokeCountText;
    public Text boundsText;
    public Text winText;
    public Text helpText;
    public Text hitPowerText;

    private UnityARAnchorManager unityARAnchorManager;
    private List<GameObject> spawnedObjects = new List<GameObject>(); //array used to keep track of spawned objects
    private int strokeCount;

    private void Start()
    {
        unityARAnchorManager = new UnityARAnchorManager();
        RestartGame();
    }

    private void OnGUI()
    {
        LinkedList<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();
        if (spawnedObjects.Count == 0)
        {
            if (arpags.Count > 0)
            {
                helpText.text = "";
                spawnButton.gameObject.SetActive(true);
            }
            else
            {
                helpText.text = "Move your phone to a flat, well lit surface.";
                spawnButton.gameObject.SetActive(false);
            }
        }

    }

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
                StartGame();
                return true;
			}
		}
		return false;
	}
    
    // Editor use only
    public void SpawnStatic()
    {
        spawnedObjects.Add(Instantiate(hitPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        StartGame();
    }

    void StartGame()
    {
        spawnButton.gameObject.SetActive(false);
    }

    public void AddStrokes()
    {
        strokeCount += 1;
        strokeCountText.text = "Strokes: " + strokeCount;
    }

    public void WinGame()
    {
        restartButton.gameObject.GetComponentInChildren<Text>().text = "Play Again";
        winText.gameObject.SetActive(true);
        EndGame();
    }

    public void EndGame()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void RemoveCourse()
    {
        GameObject course = spawnedObjects[0];
        if (spawnedObjects.Remove(course))
        {
            Destroy(course);
        }
    }

    public void SetOutOfBounds()
    {
        restartButton.gameObject.GetComponentInChildren<Text>().text = "Restart";
        boundsText.gameObject.SetActive(true);
        EndGame();
    }

    public void RestartGame()
    {
        if (spawnedObjects.Count != 0)
        {
            RemoveCourse();
        }
        winText.gameObject.SetActive(false);
        boundsText.gameObject.SetActive(false);
        strokeCount = 0;
        strokeCountText.text = "";
        hitPowerText.text = "";
        spawnButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
    }
}