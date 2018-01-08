using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNotifications : MonoBehaviour {

	[SerializeField]
	GameNotificationData notification;

	public void SendTestNotification() {
		GameNotificationHandler.SendGameNotification(notification);
	}
}
