using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple Debug menu that show when 5 fingers held down, or F12 pressed.
/// Only possible in debug mode.
/// 
/// Allows resloading of the map scene.
/// It has a prefab panel, that sets up a bunch of UI elements to make it easier to use.
/// </summary>
public class DebugUi : MonoBehaviour {

	[SerializeField]
	GameObject debugPanel = null;
	
	[SerializeField]
	Button restartGame = null;

	[SerializeField]
	Button reloadScene = null;
	
	[SerializeField]
	Button fakeWin = null;
	
	[SerializeField]
	Button closeDebug = null;

	bool isPanelVisible = false;

	/// <summary>
	/// Adds the listeners to the buttons.
	/// </summary>
	private void Start() {
		debugPanel.SetActive(false);
		restartGame.onClick.AddListener(RestartGame);
		
		reloadScene.onClick.AddListener(ReloadScene);
				
		fakeWin.onClick.AddListener(FakeWin);
		closeDebug.onClick.AddListener(CloseDebugPanel);
	}

	/// <summary>
	/// Make Sure all Event Listeners have been removed when destroying.
	/// </summary>
	private void OnDestroy() {
		restartGame.onClick.RemoveListener(RestartGame);
		reloadScene.onClick.RemoveListener(ReloadScene);		
		fakeWin.onClick.RemoveListener(FakeWin);
		closeDebug.onClick.RemoveListener(CloseDebugPanel);
	}

	/// <summary>
	/// When 5 fingers or f12 pressed, the debug panel becomes visible.
	/// </summary>
	private void Update() {
		if ((Input.touchCount >= 5 || Input.GetKeyDown(KeyCode.F12) && !isPanelVisible)) {
			isPanelVisible = true;
			debugPanel.SetActive(true);
		}
	}

	/// <summary>
	/// Restart the game from the start.
	/// </summary>
	private void RestartGame() {
		CloseDebugPanel();
		SceneManager.LoadSceneAsync("Splash_Scene");
	}

	/// <summary>
	/// Reload the current Scene.
	/// </summary>
	private void ReloadScene() {
		CloseDebugPanel();
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
	}

	/// <summary>
	/// Close the debug panel.
	/// </summary>
	private void CloseDebugPanel() {
		isPanelVisible = false;
		debugPanel.SetActive(false);
	}

	/// <summary>
	/// Launches an event to simulate a win.
	/// </summary>
	private void FakeWin() {
		CloseDebugPanel();
		EventManager.ShipFixed();
	}	
}
