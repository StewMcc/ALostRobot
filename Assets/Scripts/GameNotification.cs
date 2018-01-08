using UnityEngine;
using UnityEngine.UI;

public class GameNotification : MonoBehaviour {

	[SerializeField]
	CanvasGroup notificationGroup;

	[SerializeField]
	Image iconImage;

	[SerializeField]
	Text title;

	float timeCreated_ = 0;

	bool isActive_ = false;

	public bool IsActive() {
		return isActive_;
	}

	/// <summary>
	/// Initializes the Notification with the data, and sets the initial animation settings.
	/// </summary>
	/// <param name="data"> New Notification Data to set</param>
	/// <param name="timeNotificationCreated"> TIme at which the notification was created, controls when transitions happen. </param>
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
		notificationGroup.alpha = 0.0f;
		timeCreated_ = timeNotificationCreated;
		isActive_ = true;
	}

	/// <summary>
	/// Updates the transition state of the notification between fading in/out and showing.
	/// </summary>
	/// <param name="currentTime"> The current game time (Time.time) </param>
	/// <param name="fadeInTime"> How long it should take to fade in the notification in seconds. </param>
	/// <param name="showTime"> The time in seconds it should show for after it has faded in. </param>
	/// <param name="fadeOutTime"> How long it should take to fade out in seconds. </param>
	public void UpdateState(float currentTime, float fadeInTime, float showTime, float fadeOutTime) {
		float timeAlive = currentTime - timeCreated_;

		if (timeAlive > (fadeInTime + showTime)) {

			float percentageComplete = (timeAlive - (fadeInTime + showTime)) / fadeOutTime;
			notificationGroup.alpha = 1.0f - percentageComplete;

			if (percentageComplete > 1.0f) {
				isActive_ = false;
			}
		} else {
			float percentageComplete = timeAlive / fadeOutTime;
			notificationGroup.alpha = percentageComplete;
		}
	}

	private void Start() {
		notificationGroup.alpha = 0;
	}

}
