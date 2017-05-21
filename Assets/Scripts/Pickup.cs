using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Pickup : MonoBehaviour {

	public enum ItemType {
		tryytium, skudian, nusluathil, criasium, ublyx, zushese, vufrum, eshian, kreasten,
		qostralt
	};

	[SerializeField]
	private IdleAnimation pickUpAnimation = null;

	[SerializeField]
	private string garbageName = "DefaultName";

	[SerializeField]
	private ItemType itemType = ItemType.tryytium;

	[SerializeField]
	private string sensibleName = "DefaultName";

	[SerializeField]
	private float itemHeightOffsetWhenPickedUp = 1.0f;

	[SerializeField]
	private Canvas interactableCanvas = null;
	[SerializeField]
	private Text itemNameText =null;
	[SerializeField]
	private GameObject iconModel = null;

	private Vector3 initialPosition_ = Vector3.zero;

	private Vector3 droppedTargetPosition_ = Vector3.zero;

	private bool hasRespawned_ = false;

	private ParticleSystem dropEffect_ = null;

	/// <summary>
	/// Saves initial pos and disables Popup
	/// </summary>
	private void Start() {
		HidePopUp();
		initialPosition_ = transform.position;
		name = garbageName;
		dropEffect_ = GetComponentInChildren<ParticleSystem>();
		itemNameText.text = sensibleName;
	}

#if UNITY_EDITOR
	/// <summary>
	/// Draws Gizmo of item name in editor.
	/// </summary>
	void OnValidate() {
		if (itemNameText) {
			itemNameText.text = sensibleName;
		}
	}
#endif

	/// <summary>
	/// Height Offset for position above player, to stop clipping.
	/// </summary>
	/// <returns></returns>
	public float ItemheightOffset() {
		return itemHeightOffsetWhenPickedUp;
	}

	/// <summary>
	/// Enables the popup.
	/// </summary>
	public void ShowPopUp() {
		interactableCanvas.enabled = true;
		if (hasRespawned_) {
			itemNameText.enabled = true;
		}
		iconModel.SetActive(true);
		SoundManager.PlayEvent("Item_PopUp", gameObject);
	}

	/// <summary>
	/// Hides the popup.
	/// </summary>
	public void HidePopUp() {
		interactableCanvas.enabled = false;
		itemNameText.enabled = false;
		iconModel.SetActive(false);
		SoundManager.PlayEvent("Item_PopUp", gameObject);
	}

	/// <summary>
	/// Returns the currently available name.
	/// Garbage name or variation, or the full name.
	/// </summary>
	/// <returns></returns>
	public string Name() {
		if (hasRespawned_) {
			return sensibleName;
		}
		else {
			return garbageName;
		}
	}

	/// <summary>
	/// Pauses the animation of the pickup item.
	/// </summary>
	public void ItemPickedUp() {
		pickUpAnimation.PauseAnimation();
	}

	/// <summary>
	/// Drops the item at a random position near the player on the NavMesh.	
	/// </summary>
	public void ItemDropped() {
		if (NavMeshUtil.RandomPointOnNavMesh(transform.position, 2, out droppedTargetPosition_)) {
			droppedTargetPosition_.y += initialPosition_.y;
			transform.position = droppedTargetPosition_;
		}

		pickUpAnimation.ResumeAnimation();
		dropEffect_.Play();
		SoundManager.PlayEvent("Item_PutDown", gameObject);
	}

	/// <summary>
	/// Returns the type of the item	
	/// </summary>
	public ItemType CheckItemType() {
		return itemType;
	}

	/// <summary>
	/// Respawns the item at its initial position.
	/// </summary>
	public void Respawn() {
		transform.position = initialPosition_;
		pickUpAnimation.ResumeAnimation();
		hasRespawned_ = true;
	}

	/// <summary>
	/// Wether the item has been respawned yet.
	/// </summary>
	/// <returns> True if it has. </returns>
	public bool HasRespawned() {
		return hasRespawned_;
	}

}
