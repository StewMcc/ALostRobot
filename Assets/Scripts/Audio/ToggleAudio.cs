using UnityEngine;

public class ToggleAudio : MonoBehaviour {

	[SerializeField]
	private MeshRenderer musicMutedIcon = null;
	[SerializeField]
	private MeshRenderer sfxMutedIcon = null;
	
	private void Start() {

		// set up initial state accordingly.
		if (SoundManager.IsMusicEnabled()) {
			SoundManager.SetState(SoundManager.kMusicStateName, SoundManager.kDefault);
			musicMutedIcon.enabled = false;
		}else {
			SoundManager.SetState(SoundManager.kMusicStateName, SoundManager.kMuted);
			musicMutedIcon.enabled = true;
		}

		if (SoundManager.IsSfxAmbienceEnabled()) {
			SoundManager.SetState(SoundManager.kSfxStateName, SoundManager.kDefault);
			sfxMutedIcon.enabled = false;
		}else {
			SoundManager.SetState(SoundManager.kSfxStateName, SoundManager.kMuted);
			sfxMutedIcon.enabled = true;
		}
	}

	public void ToggleMusic() {
		if (SoundManager.IsMusicEnabled()) {
			SoundManager.SetState(SoundManager.kMusicStateName, SoundManager.kMuted);
			SoundManager.SetMusicEnabled(false);
			musicMutedIcon.enabled = true;
		}
		else {
			SoundManager.SetState(SoundManager.kMusicStateName, SoundManager.kDefault);
			SoundManager.SetMusicEnabled(true);
			musicMutedIcon.enabled = false;
		}
	}

	public void ToggleSfx() {
		if (SoundManager.IsSfxAmbienceEnabled()) {
			SoundManager.SetState(SoundManager.kSfxStateName, SoundManager.kMuted);
			SoundManager.SetSfxAmbienceEnabled(false);
			sfxMutedIcon.enabled = true;
		}
		else {
			SoundManager.SetState(SoundManager.kSfxStateName, SoundManager.kDefault);
			SoundManager.SetSfxAmbienceEnabled(true);
			sfxMutedIcon.enabled = false;
			
		}
	}

}
