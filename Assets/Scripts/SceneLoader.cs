﻿using UnityEngine;

public class SceneLoader : MonoBehaviour {

	/// <summary>
	/// Loads the provided scene.
	/// Ensure scene is added to build list.
	/// </summary>
	/// <param name="sceneName"> Name of scene to load. </param>
	public void LoadScene(string sceneName) {
		LoadingTransitionController.AnimatedLoadSceneAsync(sceneName, "LoadingSpinnerScene");
	}
}