using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameNotificationData {
	public string notificationTitle;
	public Sprite iconSprite;
}

public class GameNotificationHandler : MonoBehaviour {


	public delegate void EventHandlerNotification(GameNotificationData data);
	/// <summary>
	/// Event to listen to for when a GameNotification as been sent.
	/// </summary>
	public static event EventHandlerNotification OnRecieveGameNotification;

	[Tooltip("Fade in time for notifications in seconds")]
	[SerializeField]
	float fadeInTime = 0.5f;

	[Tooltip("Fade out time for notifications in seconds")]
	[SerializeField]
	float fadeOutTime = 0.5f;

	[Tooltip("Minimum Show Time for notifications in seconds")]
	[SerializeField]
	float minimumShowTime = 4.0f;

	[SerializeField]
	GameNotification notification;

	private Queue<GameNotificationData> notificationBackLog = new Queue<GameNotificationData>();

	/// <summary>
	/// Called to send a notification to the GameNotification UI.
	/// </summary>
	public static void SendGameNotification(GameNotificationData data) {
		// notify all listeners to event.
		if (OnRecieveGameNotification != null) {
			OnRecieveGameNotification(data);
		}
	}

	private void Start() {
		OnRecieveGameNotification += ProcessGameNotification;
	}

	private void OnDestroy() {
		OnRecieveGameNotification -= ProcessGameNotification;
	}

	private void Update() {
		if (notification.IsActive()) {
			notification.FadeOut(Time.time, fadeInTime, minimumShowTime, fadeOutTime);
		} else {
			if (notificationBackLog.Count > 0) {
				GameNotificationData notificationData = notificationBackLog.Dequeue();
				notification.InitGameNotification(notificationData, Time.time);
			}
		}
	}

	private void ProcessGameNotification(GameNotificationData data) {
		notificationBackLog.Enqueue(data);
	}

}
