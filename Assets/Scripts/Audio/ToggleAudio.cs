using UnityEngine;

public class ToggleAudio : MonoBehaviour {

	[SerializeField]
	private MeshRenderer musicMutedIcon = null;
	[SerializeField]
	private MeshRenderer sfxMutedIcon = null;

	private void Start() {
		// set up initial state accordingly.
		if (SoundManager.IsMusicEnabled()) {
			musicMutedIcon.enabled = false;
		} else {
			musicMutedIcon.enabled = true;
		}

		if (SoundManager.IsSfxAmbienceEnabled()) {
			sfxMutedIcon.enabled = false;
		} else {
			sfxMutedIcon.enabled = true;
		}
	}

	public void ToggleMusic() {
		if (SoundManager.IsMusicEnabled()) {
			musicMutedIcon.enabled = true;
		} else {
			musicMutedIcon.enabled = false;
		}
		SoundManager.ToggleMusic();
	}

	public void ToggleSfx() {
		if (SoundManager.IsSfxAmbienceEnabled()) {
			sfxMutedIcon.enabled = true;
		} else {
			sfxMutedIcon.enabled = false;
		}
		SoundManager.ToggleSfx();
	}

}
