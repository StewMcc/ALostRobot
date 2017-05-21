using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DistressBeaconController : MonoBehaviour {
	[SerializeField]
	private float waitForWinDelay = 4.0f;

	ParticleSystem particleBeam_ = null;

	private void Start() {
		particleBeam_ = GetComponent<ParticleSystem>();

	}
	private void OnEnable() {
		EventManager.OnCameraInPosition += OnCameraInPosition;
	}

	private void OnDisable() {
		EventManager.OnCameraInPosition -= OnCameraInPosition;
	}

	/// <summary>
	/// Displays the win screen when the game is completed.
	/// Hides the HUD, and changes the audio over to just the distress beacon.
	/// </summary>
	private void OnCameraInPosition() {
		// Removes the listener ensuring it doesnt accidentally get called twice.
		EventManager.OnCameraInPosition -= OnCameraInPosition;

		particleBeam_.Play();

		// stops all other music before starting the beam audio.
		SoundManager.StopAllEvents();
		SoundManager.PlayEvent("Sonar_Beam", gameObject);

		StartCoroutine(DelayedGameCompletion());
	}

	/// <summary>
	/// Delays the game completion to allow for the beam animation to finish.
	/// </summary>
	private IEnumerator DelayedGameCompletion() {

		yield return new WaitForSeconds(waitForWinDelay);
		EventManager.GameCompleted();
	}
}
