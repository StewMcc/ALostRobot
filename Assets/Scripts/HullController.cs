using UnityEngine;

public class HullController : MonoBehaviour {

	[SerializeField]
	GameObject dropButton = null;

	[SerializeField]
	GameObject hullModels = null;

	[SerializeField]
	float transitionTime = 3;

	[SerializeField]
	Transform downTransform = null;

	[SerializeField]
	Transform upTransform = null;

	private float startTime_ = 0;

	private Vector3 startPosition_ = Vector3.zero;

	private Vector3 endPosition_ = Vector3.zero;

	bool isMoving_ = false;

	bool isUp_ = false;

	void Start() {
		// put the ship model in the up position.
		hullModels.transform.position = upTransform.position;
		hullModels.SetActive(false);
		isUp_ = true;
	}
	
	private void Update() {
		if (isMoving_) {
			hullModels.transform.position = MathUtil.SmoothLerp(
				startPosition_,
				endPosition_,
				startTime_,
				transitionTime,
				out isMoving_);
			if(isUp_ && !isMoving_) {
				hullModels.SetActive(false);
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			MoveUp();
			EventManager.NameChanged(EventManager.NameUpdateType.ShipName);
			dropButton.SetActive(false);
		}
	}
	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			MoveDown();
			dropButton.SetActive(true);
			EventManager.NameChanged(EventManager.NameUpdateType.WorldName);
		}
	}


	/// <summary>
	/// Starts the animation for moving the Spaceship outer hull out of view.
	/// </summary>
	public void MoveUp() {
		if (!isUp_) {
			isUp_ = true;
			isMoving_ = true;
			startTime_ = Time.time;
			startPosition_ = downTransform.position;
			endPosition_ = upTransform.position;
		}
	}

	/// <summary>
	/// Starts the animation for moving the Spaceship outer hull in to view.
	/// </summary>
	public void MoveDown() {
		if (isUp_) {
			hullModels.SetActive(true);
			isUp_ = false;
			isMoving_ = true;
			startTime_ = Time.time;
			startPosition_ = upTransform.position;
			endPosition_ = downTransform.position;
		}
	}
}
