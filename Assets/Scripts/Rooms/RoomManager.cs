using UnityEngine;

/// <summary>
/// Sets up all child rooms, whilst handling the breaking of connetected.
/// Also handles checking if all the rooms are fixed.
/// </summary>
public class RoomManager : MonoBehaviour {

	[SerializeField]
	private Material defaultMaterial = null;
	[SerializeField]
	private Material errorMaterial = null;
	[SerializeField]
	private Material successMaterial = null;

	[SerializeField]
	private int numberOfRoomsToBreak = 2;

	[SerializeField]
	GameNotificationData roomBreakNotification = new GameNotificationData();

	private Room[] rooms_ = null;

	private int maxAttempts_ = 6;

	private void Start() {
		rooms_ = GetComponentsInChildren<Room>();

		for (int i = 0; i < rooms_.Length; i++) {
			rooms_[i].Initialise(i);
		}
	}

	private void OnEnable() {
		EventManager.OnPossibleGameCompletion += CheckShipFixed;
	}

	private void OnDisable() {
		EventManager.OnPossibleGameCompletion -= CheckShipFixed;
	}

	public Material DefaultMaterial() {
		return defaultMaterial;
	}

	public Material ErrorMaterial() {
		return errorMaterial;
	}

	public Material SuccessMaterial() {
		return successMaterial;
	}

	/// <summary>
	/// The Room manager picks a random room to break.
	///
	/// Will not break room 0, or <seealso cref="callingRoomNumber"/>.
	/// </summary>
	/// <param name="callingRoomNumber"> The room id it is being called from, i.e. room not worth breaking again.</param>
	public void BreakRandomRoom(int callingRoomNumber) {
		Random.InitState(System.DateTime.Now.Millisecond);

		int attempt = 0;
		int roomToBreak = callingRoomNumber;

		for (int i = 0; i < numberOfRoomsToBreak; i++) {
			roomToBreak = Random.Range(1, rooms_.Length);
			attempt = 0;
			// keeps looping until it runs out of attempts or gets a valid number.
			while (roomToBreak == callingRoomNumber && attempt < maxAttempts_) {
				roomToBreak = Random.Range(1, rooms_.Length);
				attempt++;
			}
			BreakRoom(roomToBreak);
		}
	}

	/// <summary>
	/// Breaks the provided room.
	/// True if it manages to break a room.
	/// </summary>
	/// <param name="roomNumber"> The room to break. </param>
	public bool BreakRoom(int roomNumber) {
		// only breaks valid room numbers
		if (roomNumber > 0 && roomNumber < rooms_.Length) {
			if (rooms_[roomNumber].IsFixed()) {
				// about to break a new room so notify user.
				// fill in notification with room name and default sprite.
				GameNotificationData data;
				data.notificationTitle = rooms_[roomNumber].Name() + roomBreakNotification.notificationTitle;
				data.iconSprite = roomBreakNotification.iconSprite;
				GameNotificationHandler.SendGameNotification(data);
			}
			rooms_[roomNumber].Break();

			return true;
		}
		return false;
	}

	/// <summary>
	/// Checks if all the rooms_ are fixed, if they are fires the Ship Fixed message.
	/// </summary>
	private void CheckShipFixed() {
		bool hasBrokenRoom = false;
		foreach (Room room in rooms_) {
			if (!room.IsFixed()) {
				hasBrokenRoom = true;
			}
		}
		if (!hasBrokenRoom) {
			EventManager.ShipFixed();
		}
	}
}
