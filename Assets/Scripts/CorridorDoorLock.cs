using UnityEngine;

public class CorridorDoorLock : Door {

	private bool isDoorLocked_ = true;

	private void OnEnable() {
		EventManager.OnPowerRoomFixed += UnlockDoor;
	}

	private void OnDisable() {
		EventManager.OnPowerRoomFixed -= UnlockDoor;
	}
	
	/// <summary>
	/// Unlocks and opens the door, curently the door just stays open once opened.
	/// </summary>
	private void UnlockDoor() {
		isDoorLocked_ = false;
		OpenDoor();
	}

	/// <summary>
	/// Override so negative noise plays when they can't open the door.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if (isDoorLocked_) {
				SoundManager.PlayEvent("Item_Port_Negative", gameObject);
			}
		}
	}
	/// <summary>
	/// Overriden as only what the doors to be opened, not closed atm.	
	/// Makes it more obvious when the corridors open up.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other) {
		
	}
}
