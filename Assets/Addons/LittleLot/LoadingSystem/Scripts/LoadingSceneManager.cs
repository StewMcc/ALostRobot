using UnityEngine;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// LoadingSceneManager.
	/// 
	/// Used for destroying the LoadingSceneManager game object, and describing the type of loading scene.
	/// <seealso cref="/LittleLot/LoadingSystem/Prefabs/LoadingRoot.prefab"/>
	/// </summary>
	public class LoadingSceneManager : MonoBehaviour {

		[Tooltip("Whether the loading scene uses 3D objects and lighting, will optimize loading to handle.")]
		[SerializeField]
		private bool is3dLoadingScene = false;

		/// <summary>
		/// Destroys the game objects releated to the loading scene.
		/// </summary>
		public void UnloadLoadingScene() {
			Destroy(gameObject);
		}

		/// <summary>
		/// Whether the transition controller should handle this as a 3d loading scene.
		/// This informs it to use the system that better handles lighting edge cases,
		/// but at the cost of beng in fade out screen during last 10% of load (awake/start)'s
		/// </summary>
		public bool Is3dLoadingScene() {
			return is3dLoadingScene;
		}
	}
}
