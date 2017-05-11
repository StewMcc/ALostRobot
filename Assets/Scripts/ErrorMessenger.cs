using UnityEngine;

public class ErrorMessenger : MonoBehaviour {

	[SerializeField]
	ErrorMessage[] errorMessages = null;

	[SerializeField]
	float messageHeight = -100.0f;

	private int currentMessage_ = 0;

	private void Start() {
		// hide all the messages.		
		foreach (ErrorMessage message in errorMessages) {
			message.SetAlpha(0);
		}
	}

	private void OnEnable() {
		EventManager.OnRoomBroken += OnRoomError;
	}

	private void OnDisable() {
		EventManager.OnRoomBroken -= OnRoomError;
	}

	/// <summary>
	/// Displays an error message for the provided room name.
	/// Happens when ever a room broken event happens.
	/// </summary>
	/// <param name="roomName"> Room name of the error message. </param>
	private void OnRoomError(string roomName) {
		DisplayErrorMessage(roomName);
	}

	/// <summary>
	/// Takes the oldest message and updates it to the provided room name with some fluff text..
	/// </summary>
	/// <param name="roomName"> Room name</param>
	private void DisplayErrorMessage(string roomName) {
		foreach (ErrorMessage message in errorMessages) {
			message.ShiftUp(messageHeight);
		}

		errorMessages[currentMessage_].ResetPosition();
		errorMessages[currentMessage_].SetText(roomName + " Damaged");
		errorMessages[currentMessage_].SetAlpha(1);

		// ensures it is the last message.
		currentMessage_++;
		// ensures it never goes out of bounds of the array.
		if (currentMessage_ >= errorMessages.Length) {
			currentMessage_ = 0;
		}
	}

}
