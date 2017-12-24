using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour {

	[SerializeField]
	private MeshRenderer quitRenderer = null;
	[SerializeField]
	private MeshRenderer noQuitRenderer = null;

	[SerializeField]
	private Button quitButton = null;

	private void Start() {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		quitRenderer.enabled = true;
		noQuitRenderer.enabled = false;
		quitButton.enabled = true;
		quitButton.targetGraphic.enabled = true;
#else
		quitRenderer.enabled = false;
		noQuitRenderer.enabled = true;
		quitButton.enabled = false;
		quitButton.targetGraphic.enabled = false;
#endif
	}

	public void Quit() {
		Application.Quit();
	}
}
