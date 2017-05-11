using UnityEngine;

[System.Serializable]
class MovingCameraPoints {
	public Transform cameraPosition =null;
	public float transitionTime = 12.0f;
}

public class CreditsRolling : MonoBehaviour {
	[SerializeField]
	Camera sceneCamera = null;
	[SerializeField]
	MovingCameraPoints[] cameraPoints = null;

	private Vector3 startPosition_ = Vector3.zero;
	private Vector3 endPosition_ = Vector3.zero;

	private float startTime_ =0;
	private float transitionTime_ = 0;


	private bool isRollingCredits_ = false;

	private int currentCamera_ =0;

	private void OnEnable() {
		EventManager.OnGameCompletion += StartRollingCredits;
	}

	private void OnDisable() {
		EventManager.OnGameCompletion -= StartRollingCredits;
	}

	// Update is called once per frame
	void Update() {
		if (isRollingCredits_) {
			sceneCamera.transform.position = MathUtil.SmoothLerp(
					startPosition_,
					endPosition_,
					startTime_,
					transitionTime_,
					out isRollingCredits_);
			if (!isRollingCredits_) {
				currentCamera_++;
				if(currentCamera_ < cameraPoints.Length) {
					isRollingCredits_ = true;
					startTime_ = Time.time;
					transitionTime_ = cameraPoints[currentCamera_].transitionTime;
					endPosition_ = cameraPoints[currentCamera_].cameraPosition.position;
					startPosition_ = sceneCamera.transform.position;
				}
			}		
		}
	}

	/// <summary>
	/// Starts the credits rolling, by moving the camera up the credits.
	/// </summary>
	private void StartRollingCredits() {
		isRollingCredits_ = true;
		startTime_ = Time.time;
		transitionTime_ = cameraPoints[currentCamera_].transitionTime;
		endPosition_ = cameraPoints[currentCamera_].cameraPosition.position;
		startPosition_ = sceneCamera.transform.position;
	}
}
