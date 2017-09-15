using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the location of the robot in the HUD.
/// </summary>
[RequireComponent(typeof(Text))]
public class LocationNameUpdater : MonoBehaviour {

	[SerializeField]
	private string shipName = "L.A.D.Ship";

	[SerializeField]
	private string worldName = "New World Name";

	private Text locationText_ = null;

	private void Start() {
		locationText_ = GetComponent<Text>();
		locationText_.text = shipName;
	}
	private void OnEnable() {
		EventManager.OnNameChanged += OnNameChange;
	}

	private void OnDisable() {
		EventManager.OnNameChanged -= OnNameChange;
	}

	/// <summary>
	/// Changes the location name to the provided nametype or newName.
	/// </summary>
	/// <param name="newNameType"> Type of name to change to. </param>
	/// <param name="newName"> Name to be used if NameUpdateType.NewName. </param>
	private void OnNameChange(EventManager.NameUpdateType newNameType, string newName) {
		if (!locationText_) {
			locationText_ = GetComponent<Text>();
		}
		switch (newNameType) {
			case EventManager.NameUpdateType.NewName:
				locationText_.text = newName;
				break;
			case EventManager.NameUpdateType.ShipName:
				locationText_.text = shipName;
				break;
			case EventManager.NameUpdateType.WorldName:
				locationText_.text = worldName;
				break;
		}

	}
}
