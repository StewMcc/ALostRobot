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

	[SerializeField]
	GameNotification notificationPrefab = null;

	[SerializeField]
	int maxNotifications = 4;

	GameNotification[] notifications;

	int currentNotification_ = 0;

	private Queue<GameNotificationData> notificationBackLog_ = new Queue<GameNotificationData>();

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
		// Instantiate pool of notifications.
		notifications = new GameNotification[maxNotifications];
		for (int i = 0; i < maxNotifications; i++) {
			notifications[i] = Instantiate(notificationPrefab, transform);
		}
	}

	private void OnDestroy() {
		OnRecieveGameNotification -= ProcessGameNotification;
	}

	private void Update() {
		float currentTime = Time.time;
		for (int i = 0; i < maxNotifications; i++) {
			if (notifications[i].IsActive()) {
				notifications[i].UpdateState(currentTime);
			} else {
				if (!notifications[currentNotification_].IsMovingUp()) {
					if (notificationBackLog_.Count > 0) {
						GameNotificationData notificationData = notificationBackLog_.Dequeue();
						notifications[i].InitGameNotification(notificationData, currentTime);
						currentNotification_ = i;
						MoveAllNotificationsUp(currentTime);
					}
				}
			}
		}
	}

	/// <summary>
	/// MoveAll the notifications up one, creating space for a new notification.
	/// </summary>
	/// <param name="currentTime"> The time at which the Move started. </param>
	private void MoveAllNotificationsUp(float currentTime) {
		for (int i = 0; i < maxNotifications; i++) {
			if (notifications[i].IsActive()) {
				notifications[i].MoveUp(currentTime);
			}
		}
	}

	/// <summary>
	/// Store Game Notification Data so can be processed when NotificationUi is ready.
	/// </summary>
	/// <param name="data"> New notification Data to display. </param>
	private void ProcessGameNotification(GameNotificationData data) {
		notificationBackLog_.Enqueue(data);
	}

}
