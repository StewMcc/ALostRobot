using UnityEngine;

/// <summary>
/// Stops all events and starts new background music.
/// </summary>
public class BackgroundMusicChanger : MonoBehaviour {

	[SerializeField]
	private AK.Wwise.Event backgroundMusicEvent;

	[SerializeField]
	private SoundManager.TriggerEvent triggerEvent = SoundManager.TriggerEvent.None;

	[SerializeField]
	private bool stopAllOtherEvents = false;

	[SerializeField]
	private bool forceRestart = false;

	private static uint currentBackgroundID = AkSoundEngine.AK_INVALID_PLAYING_ID;

	private void Awake() {
		if (triggerEvent == SoundManager.TriggerEvent.Awake) {
			ChangeBackgroundMusic();
		}
	}
	private void Start() {
		if (triggerEvent == SoundManager.TriggerEvent.Start) {
			ChangeBackgroundMusic();
		}
	}
	private void OnDestroy() {
		if (triggerEvent == SoundManager.TriggerEvent.OnDestroy) {
			ChangeBackgroundMusic();
		}
	}

	/// <summary>
	/// Stops the currently running background music before changing the background music.
	/// </summary>
	public void ChangeBackgroundMusic() {

		if (currentBackgroundID != AkSoundEngine.AK_INVALID_PLAYING_ID) {
			SoundManager.StopEvent(currentBackgroundID, 100, gameObject);
		}
		if (stopAllOtherEvents) {
			SoundManager.StopAllEvents();
		}
		// if its not already playing, and we aren't for restarting the background music.
		if (currentBackgroundID != backgroundMusicEvent.ID || forceRestart) {
			currentBackgroundID = backgroundMusicEvent.Post(gameObject);
		}
	}
}
