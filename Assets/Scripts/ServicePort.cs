using UnityEngine;

public class ServicePort : MonoBehaviour {

	[SerializeField]
	Canvas interactableCanvas = null;

	[SerializeField]
	GameObject iconObject = null;

	RoomControl connectedRoom =null;

	/// <summary>
	/// Disables Popup
	/// </summary>
	private void Start() {
		connectedRoom = GetComponentInParent<RoomControl>();
		interactableCanvas.enabled = false;
		iconObject.SetActive(false);
	}

	/// <summary>
	/// Checks to see if the connected room is fixed.
	/// </summary>
	/// <returns> True if the room is fixed. </returns>
	public bool IsFixed() {
		return connectedRoom.IsRoomFixed();
	}

	/// <summary>
	/// Enables the popup.
	/// </summary>
	public void ShowPopUp() {
		interactableCanvas.enabled = true;
		iconObject.SetActive(true);
		SoundManager.PlayEvent("Item_PopUp", gameObject);
	}

	/// <summary>
	/// Hides the popup.
	/// </summary>
	public void HidePopUp() {
		interactableCanvas.enabled = false;
		iconObject.SetActive(false);
		SoundManager.PlayEvent("Item_PopUp", gameObject);
	}
}
