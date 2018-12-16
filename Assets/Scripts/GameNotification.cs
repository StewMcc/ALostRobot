using UnityEngine;
using UnityEngine.UI;

public class GameNotification : MonoBehaviour {

	[SerializeField]
	CanvasGroup notificationGroup = null;

	[SerializeField]
	Image iconImage = null;

	[SerializeField]
	Text title = null;

	[SerializeField]
	float transitionUpTime = 1.0f;

	[SerializeField]
	float fadeOutTime = 0.5f;

	[SerializeField]
	float showTime = 4.0f;

	float notificationHeight_ = 0;

	float timeCreated_ = 0;

	bool isActive_ = false;

	bool isMovingUp_ = false;

	private Vector3 initialPosition_ = Vector3.zero;

	private Vector3 moveUpStartPosition_ = Vector3.zero;

	private Vector3 moveUpEndPosition_ = Vector3.zero;

	private float moveUpStartTime_ = 0;

	public bool IsActive() {
		return isActive_;
	}

	public bool IsMovingUp() {
		return isMovingUp_;
	}

	/// <summary>
	/// Initializes the Notification with the data, and sets the initial animation settings.
	/// </summary>
	/// <param name="data"> New Notification Data to set</param>
	/// <param name="timeNotificationCreated"> Time at which the notification was created, controls when transitions happen. </param>
	public void InitGameNotification(GameNotificationData data, float timeNotificationCreated) {
		iconImage.sprite = data.iconSprite;

		Color imageColor = iconImage.color;
		// Set image alpha to 0 if no sprite.
		if (iconImage.sprite) {
			imageColor.a = 1.0f;
		} else {
			imageColor.a = 0.0f;
		}
		iconImage.color = imageColor;

		title.text = data.notificationTitle;
		notificationGroup.alpha = 1.0f;
		timeCreated_ = timeNotificationCreated;
		isActive_ = true;
		isMovingUp_ = false;

		transform.position = initialPosition_;
	}

	/// <summary>
	/// Updates the state of the notification moves it up and fades out as it dies.
	/// </summary>
	/// <param name="currentTime"> The current game time (Time.time) </param>
	public void UpdateState(float currentTime) {
		float timeAlive = currentTime - timeCreated_;

		if (timeAlive > showTime) {
			// fadeout
			float percentageComplete = (timeAlive - showTime) / fadeOutTime;
			notificationGroup.alpha = 1.0f - percentageComplete;

			if (percentageComplete > 1.0f) {
				isActive_ = false;
			}
		}

		if (isMovingUp_) {
			transform.localPosition = LittleLot.MathUtilities.SmoothLerp(
				moveUpStartPosition_,
				moveUpEndPosition_,
				moveUpStartTime_,
				transitionUpTime,
				out isMovingUp_);
		}
	}

	/// <summary>
	/// Tells the notification that it should move up.
	/// The moving of the notification itself is handled in Update
	/// </summary>
	/// <param name="startTime"> The time at which it started moving up. </param>
	public void MoveUp(float startTime) {
		moveUpStartPosition_ = transform.localPosition;

		moveUpEndPosition_ = new Vector3(
			transform.localPosition.x,
			transform.localPosition.y + notificationHeight_,
			transform.localPosition.z);

		moveUpStartTime_ = startTime;
		isMovingUp_ = true;
	}

	private void Start() {
		notificationGroup.alpha = 0;
		notificationHeight_ = GetComponent<RectTransform>().sizeDelta.y;
		initialPosition_ = transform.position;
	}
}
