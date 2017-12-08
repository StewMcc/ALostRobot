using UnityEngine;

/// <summary>
/// Scene loader will load the provided scene.
/// 
/// Will use basic async if no transition controller or use Loading Scene not set.
/// Otherwise uses loading screen Setup.
/// </summary>
public class SceneLoader : MonoBehaviour {

	[SerializeField]
	private string nextScene = "nextScene NotSet";
	[SerializeField]
	private string loadingScene = "loadingScreen NotSet";

	/// <summary>
	/// Loads the scenes set on the object.
	/// </summary>
	public void LoadScene() {
		LoadingTransitionController.AnimatedLoadSceneAsync(nextScene, loadingScene);
	}
}
