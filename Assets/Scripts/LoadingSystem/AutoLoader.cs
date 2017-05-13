using System.Collections;
using UnityEngine;


/// <summary>
/// Automatically loads the scene at the end of the frame.
/// </summary>
public class AutoLoader : MonoBehaviour {
	[SerializeField]
	string nextScene  = "Splash_Scene";
	
	/// <summary>
	/// Automatically loads the provided strings scene at the end of the frame.
	/// </summary>
	private IEnumerator Start () {
		// Ensures it happens after everything else has finished loading.
		yield return new WaitForEndOfFrame();
		LoadingTransitionController.AnimatedLoadSceneAsync(nextScene, "LoadingScene");
	}
}
