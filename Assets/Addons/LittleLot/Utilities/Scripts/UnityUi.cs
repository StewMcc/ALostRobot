using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// @StewMcc 10/10/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Selection of utility functions for use with Unity Ui.
	/// </summary>
	public class UnityUi : MonoBehaviour {

		/// <summary>
		/// Check if the mouse or first finger is over Unity UI element.
		/// </summary>
		/// <returns></returns>
		private bool IsOverUi() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
			return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#else
			return EventSystem.current.IsPointerOverGameObject();
#endif
		}
	}
}
