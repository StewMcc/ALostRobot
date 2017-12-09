using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple Debug menu that show when 5 fingers held down, or F12 pressed.
/// Only possible in debug mode.
/// 
/// Allows reloading of the map scene.
/// It has a prefab panel, that sets up a bunch of UI elements to make it easier to use.
/// </summary>
public class DebugUi : MonoBehaviour {

	[SerializeField]
	private GameObject debugPanel = null;

    [SerializeField]
    private Text version = null;

    bool isPanelVisible_ = false;

	/// <summary>
	/// Adds the listeners to the buttons.
	/// </summary>
	private void Start() {
		debugPanel.SetActive(false);

        version.text = Application.version;
    }

	/// <summary>
	/// When 5 fingers or f12 pressed, the debug panel becomes visible.
	/// </summary>
	private void Update() {
		if ((Input.touchCount >= 5 || Input.GetKeyDown(KeyCode.F12) && !isPanelVisible_)) {
			isPanelVisible_ = true;
			debugPanel.SetActive(true);
		}
	}
	
	/// <summary>
	/// Close the debug panel.
	/// </summary>
	public void CloseDebugPanel() {
		isPanelVisible_ = false;
		debugPanel.SetActive(false);
	}

}
