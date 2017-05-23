using UnityEngine;


/// <summary>
/// Sets up basic Wwise settings, and exposes various controls for Wwise statically.
/// Will stop all events, when destroyed. Use <seealso cref="DontDestroyOnLoad"/> 
/// to esnure it is not destroyed uncessarily.
/// </summary>
public class SoundManager : Singleton<SoundManager> {

	private void Start() {
		Initialise();
	}

	private void OnDestroy() {
		StopAllEvents();
	}

	/// <summary>
	/// Setup WWise and load Init Sound Bank.
	/// </summary>
	private void Initialise() {
		uint initBankID;

		// Import Initialisation Soundbank
		AkSoundEngine.LoadBank("Init", AkSoundEngine.AK_DEFAULT_POOL_ID, out initBankID);
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
}
