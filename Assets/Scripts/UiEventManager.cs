using UnityEngine;

public class UiEventManager : MonoBehaviour {

	public delegate void EventHandler();
	/// <summary>
	/// Event to listen to for when the Hud should be hidden.
	/// </summary>
	public static event EventHandler OnHideHud;

	/// <summary>
	/// Event to listen to for when the Hud should be revealed.
	/// </summary>
	public static event EventHandler OnShowHud;

	/// <summary>
	/// Should be called to Hide the players Hud
	/// </summary>
	public static void HideHud() {
		// notify all listeners to event.
		if (OnHideHud != null) {
			OnHideHud();
		}
	}

	/// <summary>
	/// Should be called to Show the players Hud
	/// </summary>
	public static void ShowHud() {
		// notify all listeners to event.
		if (OnShowHud != null) {
			OnShowHud();
		}
	}

}
