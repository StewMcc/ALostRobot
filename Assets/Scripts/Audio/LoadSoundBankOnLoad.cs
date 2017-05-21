using UnityEngine;

public class LoadSoundBankOnLoad : MonoBehaviour {
	[SerializeField]
	private string soundbankName = "Soundbank1";

	[SerializeField]
	private bool onDestroyUnload = false;

	uint bankId_;

	private void Start() {
		// Import Soundbank
		AkSoundEngine.LoadBank(soundbankName, AkSoundEngine.AK_DEFAULT_POOL_ID, out bankId_);
	}

	private void OnDestroy() {
		if (onDestroyUnload) {
			AkSoundEngine.StopAll();
			AkSoundEngine.UnloadBank(bankId_, System.IntPtr.Zero);
		}
	}

}
