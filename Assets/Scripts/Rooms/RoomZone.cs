using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RoomZone : MonoBehaviour {
	
	private string roomName = "DefaultName";

	private void Start() {
		roomName =  GetComponentInParent<Room>().Name();		
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			EventManager.NameChanged(EventManager.NameUpdateType.NewName, roomName);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			EventManager.NameChanged(EventManager.NameUpdateType.ShipName);
		}
	}

}
