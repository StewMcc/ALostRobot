
using UnityEngine;

/// <summary>
/// Singleton LoadingSceneManager.
/// Used for starting and finishing the loading animation.
/// As well as destroying the instance of the LoadingSceneManager game object.
/// Check the LoadingRoot Prefab for an example. Animation should have bool finishAnimation set.
/// </summary>
public class LoadingSceneManager : Singleton<LoadingSceneManager> {

	[Tooltip("Only required if it has an Enter and Exit Animation, if it does must have finishAnimation bool set in animator")]
	[SerializeField]
	private Animator loadingAnimator = null;

	[Tooltip("Ignored if no animator set.")]
	[SerializeField]
	private float initialAnimationTime = 0.5f;

	[Tooltip("Ignored if no animator set.")]
	[SerializeField]
	private float finishAnimationTime = 1.0f;


	/// <summary>
	/// Destroys the game objects releated to the loading scene.
	/// </summary>
	public static void UnloadLoadingScene() {
		Destroy(instance.gameObject);
	}

	/// <summary>
	/// How long the Initial animation takes.
	/// </summary>
	public static float EnterAnimationTime() {
		if (instance.loadingAnimator) {
			return instance.initialAnimationTime;
		} else {
			return 0.0f;
		}
	}

	/// <summary>
	/// How long the exit animation takes.
	/// </summary>
	public static float ExitAnimationTime() {
		if (instance.loadingAnimator) {
			// start the finish animation.
			instance.loadingAnimator.SetBool("finishAnimation", true);
			return instance.finishAnimationTime;
		} else {
			return 0.0f;
		}

	}
}
