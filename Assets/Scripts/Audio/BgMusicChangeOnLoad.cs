using UnityEngine;

public class BgMusicChangeOnLoad : MonoBehaviour {
	[SerializeField]
	private string backgroundMusic  = "Level_Music";

	/// <summary>
	/// Stops all the currently running events before changing the background music.
	/// </summary>
	private void Start() {
		SoundManager.StopAllEvents();
		SoundManager.PlayEvent(backgroundMusic, gameObject);
		
	}
}
