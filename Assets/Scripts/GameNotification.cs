using UnityEngine;
using UnityEngine.UI;

public class GameNotification : MonoBehaviour {

	[SerializeField]
	CanvasGroup notificationGroup;

	[SerializeField]
	Image iconImage;

	[SerializeField]
	Text title;

	Vector3 initialPosition = new Vector3();

	float timeCreated = 0;

	bool isActive = false;

	public bool IsActive() {
		return isActive;
	}

	public void InitGameNotification(GameNotificationData data, float timeNotificationCreated) {
		iconImage.sprite = data.iconSprite;
		title.text = data.notificationTitle;
		notificationGroup.alpha = 0.0f;
		transform.position = initialPosition;
		timeCreated = timeNotificationCreated;
		isActive = true;
	}

	public void FadeOut(float currentTime, float fadeInTime, float showTime, float fadeOutTime) {
		float timeAlive = currentTime - timeCreated;

		if (timeAlive > (fadeInTime + showTime)) {

			float percentageComplete = (timeAlive - (fadeInTime + showTime)) / fadeOutTime;
			notificationGroup.alpha = 1.0f - percentageComplete;

			if (percentageComplete > 1.0f) {
				isActive = false;
			}
		} else {
			float percentageComplete = timeAlive / fadeOutTime;
			notificationGroup.alpha = percentageComplete;
		}
	}

	private void Start() {
		initialPosition = transform.position;
		notificationGroup.alpha = 0;
	}

}
