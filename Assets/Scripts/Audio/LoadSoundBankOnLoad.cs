using UnityEngine;

public class LoadSoundBankOnLoad : MonoBehaviour {
	[SerializeField]
	private string soundbankName = "Soundbank1";

	[SerializeField]
	bool onDestroyUnload = false;

	uint bankID;

	private void Start () {
		// Import Soundbank
		AkSoundEngine.LoadBank(soundbankName, AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
	}

	private void OnDestroy() {
		if (onDestroyUnload) {
			AkSoundEngine.StopAll();
			AkSoundEngine.UnloadBank(bankID,System.IntPtr.Zero);
		}
	}

}
