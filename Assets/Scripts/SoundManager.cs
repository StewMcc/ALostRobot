using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField]
	private string soundbankName = "Soundbank1";

	[SerializeField]
	string backgroundMusic  = "Level_Music";
	
	private void Start() {
		Initialise();
		PlayEvent(backgroundMusic, gameObject);
	}

	private void OnDestroy() {
		StopAllEvents();
	}

	/// <summary>
	/// Setup WWise and load Sound Banks.
	/// </summary>
	private void Initialise() {
		uint bankID;
		uint initBankID;

		// Import Initialisation Soundbank
		AkSoundEngine.LoadBank("Init", AkSoundEngine.AK_DEFAULT_POOL_ID, out initBankID);
		// Import Soundbank
		AkSoundEngine.LoadBank(soundbankName, AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
	}
	
	/// <summary>
	/// Play Specific Event.
	/// </summary>
	/// <param name="eventName"> Name of the event to stop. </param>	
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
