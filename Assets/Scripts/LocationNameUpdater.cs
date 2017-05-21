using UnityEngine;
using UnityEngine.UI;

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
		switch (newNameType) {
			case EventManager.NameUpdateType.NewName:
				//SoundManager.SetSwitch(gameObject, "Landscape_States", "Landscape_Shuttle_State");
				//SoundManager.StopEvent("Landscape_Planet", 0, gameObject);
				//SoundManager.PlayEvent("Landscape_Shuttle", gameObject);
				locationText_.text = newName;
				break;
			case EventManager.NameUpdateType.ShipName:
				//SoundManager.SetSwitch(gameObject, "Landscape_States", "Landscape_Shuttle_State");
				SoundManager.StopEvent("Landscape_Planet", 0, gameObject);
				SoundManager.StopEvent("Landscape_Shuttle", 0, gameObject);
				SoundManager.PlayEvent("Landscape_Shuttle", gameObject);
				locationText_.text = shipName;
				break;
			case EventManager.NameUpdateType.WorldName:
				// SoundManager.SetSwitch(gameObject, "Landscape_States", "Landscape_Outside_State");
				SoundManager.StopEvent("Landscape_Shuttle", 0, gameObject);
				SoundManager.StopEvent("Landscape_Planet", 0, gameObject);
				SoundManager.PlayEvent("Landscape_Planet", gameObject);
				locationText_.text = worldName;
				break;
		}

	}
}
