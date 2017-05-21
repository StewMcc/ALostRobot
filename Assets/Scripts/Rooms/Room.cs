using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour {
	[SerializeField]
	GameObject errorIcon = null;

	[SerializeField]
	private Pickup.ItemType correctItem = Pickup.ItemType.tryytium;
	[SerializeField]
	private string roomName = "DefaultName";

	[SerializeField]
	private bool isFixed = true;

	// this will be removed when the room meshes scales etc, are decided on, and we start merging things together.
	// atm it is seperate to allow for easy modifying and testing of the room assets.
	[SerializeField]
	GameObject roomMeshParent =null;

	private RoomManager roomManager = null;

	
	private int roomId = -1;
	private Renderer[] roomMeshParentRenderers = null;
	private Renderer[] childrenRenderers = null;
	private float successDuration = 2.0f;

	public bool IsFixed() {
		return isFixed;
	}

	public string Name() {
		return roomName;
	}

	public void Initialise(int id, int[] roomsToBreak = null) {		
		roomManager = GetComponentInParent<RoomManager>();
		roomId = id;

		childrenRenderers = GetComponentsInChildren<Renderer>();
		if (roomMeshParent) {
			roomMeshParentRenderers = roomMeshParent.GetComponentsInChildren<Renderer>();
		}
		if (isFixed) {
			errorIcon.SetActive(false);
			ChangeAllRenderersTextures(roomManager.DefaultTexture());
		}
		else {			
			errorIcon.SetActive(true);
			ChangeAllRenderersTextures(roomManager.ErrorTexture());
		}
	}

	public void TryFixRoom(Pickup item) {
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

			ChangeAllRenderersTextures(roomManager.ErrorTexture());
		}
	}

	private void Fix() {
		if (roomId == 0) {
			EventManager.PowerRoomFixed();
		}
		errorIcon.SetActive(false);
		// success the room is fixed
		isFixed = true;
		// Tell everyone the ship might be fixed.
		EventManager.PossibleCompletion();
		// Change to the success texture for a short while.
		StartCoroutine(ShowSuccess());
	}

	private void BreakOtherRooms() {
		roomManager.BreakRoom(roomId);		
	}

	private IEnumerator ShowSuccess() {
		
		ChangeAllRenderersTextures(roomManager.SuccessTexture());

		yield return new WaitForSeconds(successDuration);

		ChangeAllRenderersTextures(roomManager.DefaultTexture());
		
		yield return null;
	}
	
	private void ChangeAllRenderersTextures(Texture newTexture) {
		foreach (Renderer rend in childrenRenderers) {
			rend.material.mainTexture = newTexture;
		}
		if (roomMeshParent) {
			foreach (Renderer rend in roomMeshParentRenderers) {
				rend.material.mainTexture = newTexture;
			}
		}
	}
}
