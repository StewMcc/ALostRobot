using UnityEngine;

/// <summary>
/// Sets up basic Wwise settings, and exposes various controls for Wwise statically.
/// Will stop all events, when destroyed. 
/// </summary>
public class SoundManager : LittleLot.Singleton<SoundManager> {
	public enum TriggerEvent {
		OnDestroy,
		Start,
		Awake,
		None
	}
	private const string kMusicStateName = "MusicAudioBus";
	private const string kSfxStateName = "SfxAmbienceBus";

	private bool isMusicEnabled_ = true;
	private bool isSfxEnabled_ = true;


	/// <summary>
	/// Setup WWise and load Init Sound Bank.
	/// </summary>
	private void Start() {
		uint initBankID;

		// Import Initialisation Soundbank
		AkSoundEngine.LoadBank("Init", AkSoundEngine.AK_DEFAULT_POOL_ID, out initBankID);

		isMusicEnabled_ = (PlayerPrefs.GetInt(kMusicStateName, 1) == 1) ? true : false;
		isSfxEnabled_ = (PlayerPrefs.GetInt(kSfxStateName, 1) == 1) ? true : false;

		if (!isMusicEnabled_) {
			SetState(AKID.STATES.MUSICAUDIOBUS.GROUP, AKID.STATES.MUSICAUDIOBUS.STATE.MUTED);
		}
		if (!isSfxEnabled_) {
			SetState(AKID.STATES.SFXAMBIENCEBUS.GROUP, AKID.STATES.SFXAMBIENCEBUS.STATE.MUTED);
		}
	}

	private void OnDestroy() {
		StopAllEvents();

		PlayerPrefs.SetInt(kMusicStateName, isMusicEnabled_ ? 1 : 0);
		PlayerPrefs.SetInt(kSfxStateName, isSfxEnabled_ ? 1 : 0);
	}

	public static bool IsMusicEnabled() {
		return Instance.isMusicEnabled_;
	}
	public static bool IsSfxAmbienceEnabled() {
		return Instance.isSfxEnabled_;
	}

	public static void ToggleSfx() {
		if (Instance.isSfxEnabled_) {
			SetState(AKID.STATES.SFXAMBIENCEBUS.GROUP, AKID.STATES.MUSICAUDIOBUS.STATE.MUTED);
			Instance.isSfxEnabled_ = false;
		} else {
			SetState(AKID.STATES.SFXAMBIENCEBUS.GROUP, AKID.STATES.MUSICAUDIOBUS.STATE.DEFAULT);
			Instance.isSfxEnabled_ = true;
		}
	}

	public static void ToggleMusic() {
		if (Instance.isMusicEnabled_) {
			SetState(AKID.STATES.MUSICAUDIOBUS.GROUP, AKID.STATES.MUSICAUDIOBUS.STATE.MUTED);
			Instance.isMusicEnabled_ = false;
		} else {
			SetState(AKID.STATES.MUSICAUDIOBUS.GROUP, AKID.STATES.MUSICAUDIOBUS.STATE.DEFAULT);
			Instance.isMusicEnabled_ = true;
		}
	}

	/// <summary>
	/// Play Specific Event.
	/// </summary>
	/// <param name="eventID"> Name of the event to start playing. </param>	
	/// <param name="go"> GameObject assosciated to the event. </param>
	public static void PlayEvent(uint eventID, GameObject go) {
		AkSoundEngine.PostEvent(eventID, go);
	}

	/// <summary>
	/// Stop Specific Event.
	/// </summary>
	/// <param name="eventID"> Name of the event to stop. </param>
	/// <param name="fadeOut"> Fade out duration in MS. </param>
	/// <param name="go"> GameObject assosciated to the event. </param>
	public static void StopEvent(uint eventID, int fadeOut, GameObject go) {
		AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, go, fadeOut, AkCurveInterpolation.AkCurveInterpolation_Sine);
	}

	/// <summary>
	/// Stops All running Events .
	/// </summary>
	public static void StopAllEvents() {
		AkSoundEngine.StopAll();
	}

	/// <summary>
	/// Switch States, sets the state of a switch group.
	/// </summary>
	/// <param name="switchID"> Name of the switch group. </param>
	/// <param name="switchState"> Name of the switch. </param>
	/// /// <param name="go"> GameObject assosciated to the event. </param>
	public static void SetSwitch(uint switchID, uint switchState, GameObject go) {
		AkSoundEngine.SetSwitch(switchID, switchState, go);
	}

	/// <summary>
	/// Set RTPCs.
	/// Can consume a significant amount of the platform's memory and CPU.
	/// </summary>
	/// <param name="rtpcName"> Name of the RTPC. </param>
	/// <param name="rtpcValue"> Value to set to. </param>
	public static void SetRTPC(uint rtpcId, float rtpcValue) {
		AkSoundEngine.SetRTPCValue(rtpcId, rtpcValue);
	}

	/// <summary>
	/// Sets the state group to the state
	/// </summary>
	/// <param name="stateId"> StateGroup in which to change. </param>
	/// <param name="state"> State to change to in stateGroup. </param>
	public static void SetState(uint stateId, uint state) {
		AkSoundEngine.SetState(stateId, state);
	}
}
