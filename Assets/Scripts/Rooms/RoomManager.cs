using UnityEngine;

public class RoomManager : MonoBehaviour {

	[SerializeField]
	private Texture defaultTexture = null;
	[SerializeField]
	private Texture errorTexture = null;
	[SerializeField]
	private Texture successTexture = null;

	private Room[] rooms_ = null;

	private int maxAttempts_ =6;

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

	public Texture DefaultTexture() {
		return defaultTexture;
	}

	public Texture ErrorTexture() {
		return errorTexture;
	}

	public Texture SuccessTexture() {
		return successTexture;
	}

	/// <summary>
	/// The Room manager picks a random room to break.
	/// 
	/// Will not break room 0, or <seealso cref="callingRoomNumber"/>.
	/// </summary>
	/// <param name="callingRoomNumber"> The room id it is being called from, i.e. room not worth breaking again.</param>
	public void BreakRandomRoom(int callingRoomNumber) {
		int attempt = 0;
		int roomToBreak = Random.Range(1, rooms_.Length);

		// keeps looping until it runs out of attempts or gets a valid number.
		while (roomToBreak == callingRoomNumber && attempt < maxAttempts_) {
			roomToBreak = Random.Range(1, rooms_.Length);
			attempt++;
		}
		BreakRoom(roomToBreak);
	}

	/// <summary>
	/// Breaks the provided room.
	/// True if it manages to break a room.
	/// </summary>
	/// <param name="roomNumber"> The room to break. </param>
	public bool BreakRoom(int roomNumber) {
		// only breaks valid room numbers
		if (roomNumber > 0 && roomNumber < rooms_.Length) {
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
