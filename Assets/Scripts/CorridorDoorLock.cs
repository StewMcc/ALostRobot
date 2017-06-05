using UnityEngine;

/// <summary>
/// Locks the corridors until the powerRoom is fixed.
/// Extends <seealso cref="Door"/> so that the door wont open unless unlocked.
/// Doors unlock and open to stay open when the power room is fixed the first time.
/// </summary>
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
        SoundManager.PlayEvent("Craft_PowerBattery", gameObject);
        //SoundManager.StopEvent("Craft_Alarm", gameObject);
    }

	/// <summary>
	/// Override so negative noise plays when they can't open the door.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if (isDoorLocked_) {
				SoundManager.PlayEvent("Door_Locked", gameObject);
			}
		}
	}
	/// <summary>
	/// Overriden as only want the doors to be opened, not closed atm.	
	/// Makes it more obvious when the corridors open up.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other) {

	}
}
