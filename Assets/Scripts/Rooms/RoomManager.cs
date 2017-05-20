using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

	[SerializeField]
	private Texture defaultTexture = null;
	[SerializeField]
	private Texture errorTexture = null;
	[SerializeField]
	private Texture successTexture = null;

	private Room[] rooms = null;

	private void Start() {
		rooms = GetComponentsInChildren<Room>();
		// TODO: Generate sets of rooms to break for each room?
		// just random, shuffled list, 
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

	public Texture DefaultTexture() {
		return defaultTexture;
	}

	public Texture ErrorTexture() {
		return errorTexture;
	}

	public Texture SuccessTexture() {
		return successTexture;
	}

	public void BreakRoom(int roomNum) {
		rooms[roomNum].Break();
	}

	/// <summary>
	/// Checks if all the rooms are fixed, if they are fires the Ship Fixed message.
	/// </summary>
	private void CheckShipFixed() {
		bool hasBrokenRoom = false;
		foreach (Room room in rooms) {
			if (!room.IsFixed()) {
				hasBrokenRoom = true;
			}
		}
		if (!hasBrokenRoom) {
			//EventManager.ShipFixed();
		}
	}
}
