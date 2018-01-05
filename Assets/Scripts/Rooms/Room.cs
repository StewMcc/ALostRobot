using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the breaking and fixing of rooms.
/// Shows this by changing all the room textures dependent on state.
/// Uses the parent RoomManager to get Textures and break other rooms when it fails to be fixed.
/// </summary>
public class Room : MonoBehaviour {

	[SerializeField]
	private GameObject errorIcon = null;

	[SerializeField]
	private Pickup.PickupType correctItem = Pickup.PickupType.tryytium;

	[SerializeField]
	private string roomName = "DefaultName";

	[SerializeField]
	private bool isFixed = true;

	[SerializeField]
	private AK.Wwise.Event ambientFixedEvent;

	[SerializeField]
	private AK.Wwise.Event ambientBrokenEvent;

	[Tooltip("Object that contains all meshes we wish to show error and success textures on.")]
	[SerializeField]
	private GameObject roomMeshParent = null;

	private RoomManager roomManager_ = null;

	private int roomId_ = -1;
	private Renderer[] roomMeshParentRenderers_ = null;
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

		// Get all the renderers connected to this room and change their textures.

		if (roomMeshParent) {
			roomMeshParentRenderers_ = roomMeshParent.GetComponentsInChildren<Renderer>();
		}
		if (isFixed) {
			errorIcon.SetActive(false);
			ChangeAllRenderersTextures(roomManager_.DefaultMaterial());
			ambientFixedEvent.Post(gameObject);
		} else {
			ambientBrokenEvent.Post(gameObject);
			errorIcon.SetActive(true);
			ChangeAllRenderersTextures(roomManager_.ErrorMaterial());
		}
	}

	public void TryFix(Pickup item) {
		if (item.CheckType() == correctItem) {
			SoundManager.PlayEvent(AKID.EVENTS.ITEM_PORT_POSITIVE, gameObject);
			ambientFixedEvent.Post(gameObject);
			ambientBrokenEvent.Stop(gameObject, 100);
			Fix();
		} else {
			SoundManager.PlayEvent(AKID.EVENTS.ITEM_PORT_NEGATIVE, gameObject);
			BreakOtherRooms();
		}
	}

	public void Break() {
		if (isFixed) {
			isFixed = false;
			SoundManager.PlayEvent(AKID.EVENTS.BROKEN_ROOM, gameObject);
			ambientFixedEvent.Stop(gameObject, 100);
			ambientBrokenEvent.Post(gameObject);
			errorIcon.SetActive(true);
			ChangeAllRenderersTextures(roomManager_.ErrorMaterial());
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

		ChangeAllRenderersTextures(roomManager_.SuccessMaterial());

		yield return new WaitForSeconds(successDuration_);

		ChangeAllRenderersTextures(roomManager_.DefaultMaterial());

		yield return null;
	}

	private void ChangeAllRenderersTextures(Material newTexture) {

		if (roomMeshParent) {
			foreach (Renderer rend in roomMeshParentRenderers_) {
				rend.material = newTexture;
			}
		}
	}
}
