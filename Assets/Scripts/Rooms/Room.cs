using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour {

	[SerializeField]
	private GameObject errorIcon = null;

	[SerializeField]
	private Pickup.ItemType correctItem = Pickup.ItemType.tryytium;

	[SerializeField]
	private string roomName = "DefaultName";

	[SerializeField]
	private bool isFixed = true;

	// TODO: this will be removed when the room meshes scales etc, are decided on, and we start merging things together.
	// atm it is seperate to allow for easy modifying and testing of the room assets.
	[Tooltip("Other object that contains all meshes we wish to show error and success textures on. (Temporary, to be removed)")]
	[SerializeField]
	private GameObject roomMeshParent =null;

	private RoomManager roomManager_ = null;

	private int roomId_ = -1;
	private Renderer[] roomMeshParentRenderers_ = null;
	private Renderer[] childrenRenderers_ = null;
	private float successDuration_ = 2.0f;

	public bool IsFixed() {
		return isFixed;
	}

	public string Name() {
		return roomName;
	}

	/// <summary>
	/// Sets up the Room, and sets it and its children to the correct texture.
	/// </summary>
	/// <param name="id"> The position of this room in the room manager. </param>
	public void Initialise(int id) {
		roomManager_ = GetComponentInParent<RoomManager>();
		roomId_ = id;

		childrenRenderers_ = GetComponentsInChildren<Renderer>();
		if (roomMeshParent) {
			roomMeshParentRenderers_ = roomMeshParent.GetComponentsInChildren<Renderer>();
		}
		if (isFixed) {
			errorIcon.SetActive(false);
			ChangeAllRenderersTextures(roomManager_.DefaultTexture());
		}
		else {
			errorIcon.SetActive(true);
			ChangeAllRenderersTextures(roomManager_.ErrorTexture());
		}
	}

	public void TryFix(Pickup item) {
		if (item.CheckItemType() == correctItem) {
			SoundManager.PlayEvent("Item_Port_Positive", gameObject);
			Fix();
		}
		else {
			SoundManager.PlayEvent("Item_Port_Negative", gameObject);
			BreakOtherRooms();
		}
	}

	public void Break() {
		if (isFixed) {
			isFixed = false;
			SoundManager.PlayEvent("Broken_Room", gameObject);
			errorIcon.SetActive(true);

			ChangeAllRenderersTextures(roomManager_.ErrorTexture());
		}
	}

	private void Fix() {
		if (roomId_ == 0) {
			EventManager.PowerRoomFixed();
		}
		errorIcon.SetActive(false);
		isFixed = true;
		// Tell everyone the ship might be fixed.
		EventManager.PossibleCompletion();

		StartCoroutine(ShowSuccess());
	}

	private void BreakOtherRooms() {
		roomManager_.BreakRandomRoom(roomId_);
	}

	/// <summary>
	/// Shows the success textue for the successDuration_.
	/// </summary>
	private IEnumerator ShowSuccess() {

		ChangeAllRenderersTextures(roomManager_.SuccessTexture());

		yield return new WaitForSeconds(successDuration_);

		ChangeAllRenderersTextures(roomManager_.DefaultTexture());

		yield return null;
	}

	private void ChangeAllRenderersTextures(Texture newTexture) {
		foreach (Renderer rend in childrenRenderers_) {
			rend.material.mainTexture = newTexture;
		}
		if (roomMeshParent) {
			foreach (Renderer rend in roomMeshParentRenderers_) {
				rend.material.mainTexture = newTexture;
			}
		}
	}
}
