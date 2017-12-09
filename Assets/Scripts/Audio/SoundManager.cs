using UnityEngine;


/// <summary>
/// Sets up basic Wwise settings, and exposes various controls for Wwise statically.
/// Will stop all events, when destroyed. 
/// </summary>
public class SoundManager : Singleton<SoundManager> {
	public const string kMusicStateName = "MusicAudioBus";
	public const string kSfxStateName = "SfxAmbienceBus";
	public const string kMuted = "Muted";
	public const string kDefault = "Default";

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
	}
	
	private void OnDestroy() {
		StopAllEvents();

		PlayerPrefs.SetInt(kMusicStateName, isMusicEnabled_ ? 1 : 0);
		PlayerPrefs.SetInt(kSfxStateName, isSfxEnabled_ ? 1 : 0);
	}

	public static bool IsMusicEnabled() {
		return instance.isMusicEnabled_;
	}
	public static bool IsSfxAmbienceEnabled() {
		return instance.isSfxEnabled_;
	}
	public static void SetMusicEnabled(bool isEnabled) {
		instance.isMusicEnabled_ = isEnabled;
	}
	public static void SetSfxAmbienceEnabled(bool isEnabled) {
		instance.isSfxEnabled_ = isEnabled;
		
	}

	/// <summary>
	/// Play Specific Event.
	/// </summary>
	/// <param name="eventName"> Name of the event to start playing. </param>	
	/// <param name="go"> GameObject assosciated to the event. </param>
	public static void PlayEvent(string eventName, GameObject go) {
		AkSoundEngine.PostEvent(eventName, go);
	}

	/// <summary>
	/// Stop Specific Event.
	/// </summary>
	/// <param name="eventName"> Name of the event to stop. </param>
	/// <param name="fadeOut"> Fade out duration in MS. </param>
	/// <param name="go"> GameObject assosciated to the event. </param>
	public static void StopEvent(string eventName, int fadeOut, GameObject go) {
		uint eventID;
		eventID = AkSoundEngine.GetIDFromString(eventName);
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
	/// <param name="switchName"> Name of the switch group. </param>
	/// <param name="switchState"> Name of the switch. </param>
	/// /// <param name="go"> GameObject assosciated to the event. </param>
	public static void SetSwitch(string switchName, string switchState, GameObject go) {
		AkSoundEngine.SetSwitch(switchName, switchState, go);
	}

	/// <summary>
	/// Set RTPCs.
	/// Can consume a significant amount of the platform's memory and CPU.
	/// </summary>
	/// <param name="rtpcName"> Name of the RTPC. </param>
	/// <param name="rtpcValue"> Value to set to. </param>
	public static void SetRTPC(string rtpcName, float rtpcValue) {
		AkSoundEngine.SetRTPCValue(rtpcName, rtpcValue);
	}

	/// <summary>
	/// Sets the state group to the state
	/// </summary>
	/// <param name="stateGroup"> StateGroup in which to change. </param>
	/// <param name="stateName"> State to change to in stateGroup. </param>
	public static void SetState(string stateGroup, string stateName) {
		AkSoundEngine.SetState(stateGroup, stateName);
	}
}
