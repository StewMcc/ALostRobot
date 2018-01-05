using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {

	[SerializeField]
	private Button openPauseMenu = null;

	[SerializeField]
	private Button closePauseMenu = null;

	private void Start() {
		openPauseMenu.onClick.AddListener(OpenPauseMenu);
		closePauseMenu.onClick.AddListener(ClosePauseMenu);
		gameObject.SetActive(false);
	}

	private void OnDestroy() {
		openPauseMenu.onClick.RemoveListener(OpenPauseMenu);
		closePauseMenu.onClick.RemoveListener(ClosePauseMenu);
		Time.timeScale = 1;
	}

	private void OpenPauseMenu() {
		gameObject.SetActive(true);
		UiEventManager.HideHud();
		SoundManager.PlayEvent(AKID.EVENTS.MENU_PAUSE, gameObject);

		Time.timeScale = 0;
	}

	public void ClosePauseMenu() {
		gameObject.SetActive(false);
		UiEventManager.ShowHud();
		SoundManager.PlayEvent(AKID.EVENTS.MENU_RESUME, gameObject);
		Time.timeScale = 1;
	}

}
