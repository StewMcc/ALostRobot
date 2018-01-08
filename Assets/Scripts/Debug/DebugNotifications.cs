using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNotifications : MonoBehaviour {

	[SerializeField]
	GameNotificationData notification;

	private int notificationNumber_ = 0;

	public void SendTestNotification() {
		notificationNumber_++;
		GameNotificationData data;
		data.notificationTitle = notificationNumber_ + " " + notification.notificationTitle;
		data.iconSprite = notification.iconSprite;

		GameNotificationHandler.SendGameNotification(data);
	}

}
