using UnityEngine;

/// <summary>
/// Handles showing the correct zone name for each of the Rooms.
/// Gets room name from its parent, helps seperate out the colliders neatly.
/// </summary>
[RequireComponent(typeof(Collider))]
public class RoomZone : MonoBehaviour {

	private string roomName_ = "DefaultName";

	private void Start() {
		// get the room name from its parents room.
		roomName_ = GetComponentInParent<Room>().Name();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			EventManager.NameChanged(EventManager.NameUpdateType.NewName, roomName_);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			EventManager.NameChanged(EventManager.NameUpdateType.ShipName);
		}
	}

}
