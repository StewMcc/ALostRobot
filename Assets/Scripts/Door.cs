using UnityEngine;

/// <summary>
/// When the player enters the collider the doorModel will move towards the openTransform.
/// On exit it moves to the closedTransform.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour {
	[SerializeField]
	private GameObject doorModel = null;

	[SerializeField]
	private float transitionTime = 0.2f;
	[SerializeField]
	private Transform openTransform = null;

	[SerializeField]
	private Transform closedTransform = null;

	private float startTime_ = 0;

	private Vector3 startPosition_ = Vector3.zero;

	private Vector3 endPosition_ = Vector3.zero;

	private bool isMoving_ = false;

	void Update() {
		if (isMoving_) {
			// transition between the 2 positions.
			doorModel.transform.position = LittleLot.MathUtilities.SmoothLerp(
				startPosition_,
				endPosition_,
				startTime_,
				transitionTime,
				out isMoving_);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			OpenDoor();
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			CloseDoor();
		}
	}

	/// <summary>
	/// Starts the close door animation and plays the sound.
	/// </summary>
	public void CloseDoor() {
		isMoving_ = true;
		startTime_ = Time.time;
		startPosition_ = openTransform.position;
		endPosition_ = closedTransform.position;
		SoundManager.PlayEvent(AKID.EVENTS.CRAFT_HYDRODOOR_CLOSE, gameObject);
	}

	/// <summary>
	/// Starts the open door animation and plays the sound.
	/// </summary>
	public void OpenDoor() {
		isMoving_ = true;
		startTime_ = Time.time;
		startPosition_ = closedTransform.position;
		endPosition_ = openTransform.position;
		SoundManager.PlayEvent(AKID.EVENTS.CRAFT_HYDRODOOR_OPEN, gameObject);
	}
}
