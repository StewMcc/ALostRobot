using System.Collections;

using UnityEngine;

/// <summary>
/// Singleton LoadingSceneManager.
/// Used for starting and finishing the loading animation.
/// As well as destroying the instance of the LoadingSceneManager game object.
/// Check the LoadingRoot Prefab for an example.
/// </summary>
public class LoadingSceneManager : Singleton<LoadingSceneManager> {

	[SerializeField]
	Animator loadingAnimator = null;

	[SerializeField]
	float initialAnimationTime = 0.5f;

	[SerializeField]
	float finishAnimationTime = 1.0f;


	/// <summary>
	/// Destroys the game objects releated to the loading scene.
	/// </summary>
	public static void UnloadLoadingScene() {
		Destroy(instance.gameObject);
	}

	/// <summary>
	/// Waits for the Initial animation state to finish.
	/// </summary>
	/// <returns> IEnumerator WaitForSeconds, with the seconds of the Animation. </returns>
	public static IEnumerator WaitForInitialLoadingAnimation() {
		// wait for the initial animation sequence to finish.
		yield return new WaitForSeconds(instance.initialAnimationTime);
	}

	/// <summary>
	/// Starts the finish animation then waits for it to finish.
	/// </summary>
	/// <returns> IEnumerator WaitForSeconds, with the seconds of the Animation. </returns>
	public static IEnumerator StartFinishLoadingAnimation() {
		// start the finish animation.
		instance.loadingAnimator.SetBool("finishAnimation", true);

		// wait for the animation finish.
		yield return new WaitForSeconds(instance.finishAnimationTime);
	}
}
