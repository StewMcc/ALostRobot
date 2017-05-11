using UnityEngine;

public class RoomManager : MonoBehaviour {
	private RoomControl[] rooms;

	public Texture whiteTex;
	public Texture redTex;
	public Texture greenTex;

	// Get all rooms that are children of the room manager
	// Initialise them with their room number
	void Start() {
		rooms = GetComponentsInChildren<RoomControl>();
		for (int i = 0; i < rooms.Length; i++) {
			rooms[i].Initialise(i);
		}
	}

	private void OnEnable() {
		EventManager.OnPossibleGameCompletion += CheckShipFixed;
	}

	private void OnDisable() {
		EventManager.OnPossibleGameCompletion -= CheckShipFixed;
	}

	public int NumberOfRooms() {
		return rooms.Length;
	}

	public void BreakRoom(int roomNum) {
		rooms[roomNum].Break();
	}

	/// <summary>
	/// Checks if all the rooms are fixed, if they are fires the Ship Fixed message.
	/// </summary>
	private void CheckShipFixed() {
		bool hasBrokenRoom = false;
		foreach (RoomControl room in rooms) {
			if (!room.IsRoomFixed()) {
				hasBrokenRoom = true;
			}
		}
		if (!hasBrokenRoom) {
			EventManager.ShipFixed();
		}
	}
}
