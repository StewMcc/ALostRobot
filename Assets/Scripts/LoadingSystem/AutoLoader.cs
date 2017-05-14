using System.Collections;
using UnityEngine;


/// <summary>
/// Automatically loads the scene at the end of the frame.
/// </summary>
public class AutoLoader : MonoBehaviour {
	[SerializeField]
	string nextScene  = "Splash_Scene";
	[SerializeField]
	float delay = 2.0f;
	
	/// <summary>
	/// Automatically loads the provided strings scene at the end of the frame.
	/// </summary>
	private IEnumerator Start () {
		// Ensures it happens after everything else has finished loading.
		yield return new WaitForEndOfFrame();
		// w8 for the delay
		yield return new WaitForSeconds(delay);

		LoadingTransitionController.AnimatedLoadSceneAsync(nextScene, "LoadingSpinnerScene");
	}
}
