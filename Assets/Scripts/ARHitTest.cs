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
    public Slider forceSlider;
    public Text hitForceLabel;
    public Text strokeCountText;
    public Text boundsText;
    public Text winText;
    public Text helpText;

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
        IEnumerable<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();
        if (spawnedObjects.Count == 0)
        {
            if (arpags == null)
            {
                helpText.text = "Move your phone to a flat, well lit surface.";
                spawnButton.gameObject.SetActive(false);
            }
            else
            {
                helpText.text = "";
                spawnButton.gameObject.SetActive(true);
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
    
    // Testing only
    public void SpawnStatic()
    {
        spawnedObjects.Add(Instantiate(hitPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        StartGame();
    }

	// Fixed Update is called once per frame
	void FixedUpdate () {
		if (Input.GetMouseButtonDown(0)) { //this works with touch as well as with a mouse
            //RemoveObject (Input.mousePosition); 
		}
    }

    void StartGame()
    {
        spawnButton.gameObject.SetActive(false);
        //hitButton.gameObject.SetActive(true);
        //forceSlider.gameObject.SetActive(true);
        //hitForceLabel.gameObject.SetActive(true);
    }

    public void AddStrokes()
    {
        strokeCount += 1;
        strokeCountText.text = "Strokes: " + strokeCount;
    }

    public void WinGame()
    {
        winText.gameObject.SetActive(true);
        EndGame();
    }

    public void EndGame()
    {
        restartButton.gameObject.SetActive(true);
        //hitButton.gameObject.SetActive(false);
        //forceSlider.gameObject.SetActive(false);
        //hitForceLabel.gameObject.SetActive(false);

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
        spawnButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        //hitButton.gameObject.SetActive(false);
        //forceSlider.value = 0;
        //forceSlider.gameObject.SetActive(false);
        //hitForceLabel.gameObject.SetActive(false);
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