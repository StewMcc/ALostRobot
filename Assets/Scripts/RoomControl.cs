﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Depreceated Remove completly.
public class RoomControl : MonoBehaviour {
	[SerializeField]
	GameObject errorIcon = null;

	[SerializeField]
	private RobotController player = null;

	[SerializeField]
	private int maxRoomsToBreak = 2;
	[SerializeField]
	private Pickup.ItemType correctItem = Pickup.ItemType.tryytium;

	private List<ItemResponse> incorrectResponses = null;
	private OldRoomManager roomManager = null;
	private OldServicePort servicePort = null;
	private bool isFixed = true;
	int roomID;
	private float greenTexDuration = 2.0f;

	private Renderer[] rend = null;

	public void Initialise(int id) {
		roomManager = GetComponentInParent<OldRoomManager>();
		servicePort = GetComponentInChildren<OldServicePort>();
		roomID = id;
		int iterator = 0;
		int maxRooms = roomManager.NumberOfRooms();
		incorrectResponses = new List<ItemResponse>();
		rend = GetComponentsInChildren<Renderer>();

		// create a response for each availabe item type (- the correct item)
		for (int i = 0; i < System.Enum.GetValues(typeof(Pickup.ItemType)).Length; i++) {
			incorrectResponses.Add(new ItemResponse());
		}

		for (int response = 0; response < incorrectResponses.Count; response++) {
			// randomly select how many rooms will be broken by presenting the incorrect item
			incorrectResponses[response].Initialise(maxRoomsToBreak);

			// for each room that will be broken
			for (int room = 0; room < incorrectResponses[response].roomsToBreak.Length; room++) {
				// select a room which is not this room
				bool validRoom = false;
				while (!validRoom) {
					int roomToBreak = Random.Range(0, maxRooms);
					if (roomToBreak != roomID) {
						incorrectResponses[response].roomsToBreak[room] = roomToBreak;
						validRoom = true;
					}
				}
			}
			// move to the next item type
			iterator++;
		}

		foreach (Renderer r in rend) {
			r.material.mainTexture = roomManager.whiteTex;
		}
		// Break room without audio
		errorIcon.SetActive(true);
		
		foreach (Renderer r in rend) {
			r.material.mainTexture = roomManager.redTex;
		}
		isFixed = false;

		//if (roomID == 0) {
		//	Break();
		//}
		//else {
		//	errorIcon.SetActive(false);
		//	// success the room is fixed
		//	isFixed = true;
		//}
	}

	public void CheckItem() {
		Pickup.ItemType presentedItem = player.CurrentItem().CheckItemType();
		if (presentedItem == correctItem) {
			SoundManager.PlayEvent("Item_Port_Positive", gameObject);
			FixRoom();
		}
		else {
			SoundManager.PlayEvent("Item_Port_Negative", gameObject);
			// get a list of rooms to break and break them
			int[] roomsToBreak = incorrectResponses[(int)presentedItem].roomsToBreak;
			foreach (int roomNum in roomsToBreak) {
				roomManager.BreakRoom(roomNum);
			}
		}
		player.RespawnCurrentItem();
		servicePort.HidePopUp();
	}

	/// <summary>
	/// If the room has been fixed.
	/// </summary>
	/// <returns> True if it is fixed. </returns>
	public bool IsRoomFixed() {
		return isFixed;
	}

	/// <summary>
	/// Breaks the room.
	/// </summary>
	public void Break() {		
		errorIcon.SetActive(true);
		if (isFixed) {
			SoundManager.PlayEvent("Broken_Room", gameObject);
		}
		foreach (Renderer r in rend) {
			r.material.mainTexture = roomManager.redTex;
		}
		isFixed = false;
	}

	private void FixRoom() {
		if(roomID == 0) {
			EventManager.PowerRoomFixed();
		}
		errorIcon.SetActive(false);
		// success the room is fixed
		isFixed = true;
		// Tell everyone the ship might be fixed.
		EventManager.PossibleCompletion();
		// Change to the greenTexture
		StartCoroutine(ShowGreenTex());
	}

	private IEnumerator ShowGreenTex() {
		foreach (Renderer r in rend) {
			r.material.mainTexture = roomManager.greenTex;
		}

		yield return new WaitForSeconds(greenTexDuration);

		foreach (Renderer r in rend) {
			r.material.mainTexture = roomManager.whiteTex;
		}

		yield return null;
	}

}
