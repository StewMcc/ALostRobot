using UnityEngine;

public class EventManager : MonoBehaviour {
	public enum NameUpdateType { NewName, ShipName, WorldName };

	public delegate void EventHandler();
	/// <summary>
	/// Event to listen to for when the game might have been complted.
	/// </summary>
	public static event EventHandler OnPossibleGameCompletion;

	/// <summary>
	/// Event to listen to for when the camera is in position for the distress beacon.
	/// </summary>
	public static event EventHandler OnCameraInPosition;

	/// <summary>
	/// Event to listen to for when the game is completed.
	/// </summary>
	public static event EventHandler OnGameCompletion;

	/// <summary>
	/// Event to listen to for when the ship is fixed
	/// </summary>
	public static event EventHandler OnShipFixed;

	/// <summary>
	/// Should be called when the power room is fixed.
	/// </summary>
	public static event EventHandler OnPowerRoomFixed;


	public delegate void EventHandlerLocationName(NameUpdateType newNameType, string newName = "");

	/// <summary>
	/// Event to listen to for when locationNameChanges.
	/// </summary>
	public static event EventHandlerLocationName OnNameChanged;

	/// <summary>
	/// Should be called when a the ship might be fixed.
	/// </summary>
	public static void PossibleCompletion() {
		// notify all listeners to event.
		if (OnPossibleGameCompletion != null) {
			OnPossibleGameCompletion();
		}
	}

	/// <summary>
	/// Should be called when the Camera is ready for the distress signal to send.
	/// </summary>
	public static void CameraInPosition() {
		// notify all listeners to event.
		if (OnCameraInPosition != null) {
			OnCameraInPosition();
		}
	}

	/// <summary>
	/// Should be called when the distress signal has been sent.
	/// </summary>
	public static void GameCompleted() {
		// notify all listeners to event.
		if (OnGameCompletion != null) {
			OnGameCompletion();
		}
	}

	/// <summary>
	/// Should be called when a the ship is fixed.
	/// </summary>
	public static void ShipFixed() {
		// notify all listeners to event.
		if (OnShipFixed != null) {
			OnShipFixed();
		}
	}

	/// <summary>
	/// Should be called when a the power room is fixed.
	/// </summary>
	public static void PowerRoomFixed() {
		// notify all listeners to event.
		if (OnPowerRoomFixed != null) {
			OnPowerRoomFixed();
		}
	}

	/// <summary>
	/// Should be called when wanting to change the location name.
	/// Use NameUpdateType.NewName, in newNameType to allow newName it to be used instead. 
	/// (default newName = "")
	/// </summary>
	/// <param name="newNameType">
	/// NameUpdateType can be used to change to some default names, planet etc.
	/// </param>
	/// <param name="newName"> 
	/// New name to be set. Use NameUpdateType.NewName, in newNameType to allow it to be used.
	/// </param>
	public static void NameChanged(NameUpdateType newNameType, string newName = "") {
		// notify all listeners to event.
		if (OnNameChanged != null) {
			OnNameChanged(newNameType, newName);
		}
	}
	
}
