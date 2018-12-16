using UnityEngine;

// @StewMcc 10/12/2017
namespace LittleLot {

	/// <summary>
	/// Simple Debug menu that show when 5 fingers held down, or F12 pressed.
	/// </summary>
	public class DebugOverlay : MonoBehaviour {

		[SerializeField]
		GameObject debugPanel = null;

		private bool isPanelVisible_ = false;

		/// <summary>
		/// Adds the listeners to the buttons.
		/// </summary>
		private void Start() {
			debugPanel.SetActive(false);
		}

		/// <summary>
		/// When 5 fingers or f12 pressed, the debug panel becomes visible.
		/// </summary>
		private void Update() {
			if ((Input.touchCount >= 5 || Input.GetKeyDown(KeyCode.F12) && !isPanelVisible_)) {
				isPanelVisible_ = true;
				debugPanel.SetActive(true);
			}
		}

		/// <summary>
		/// Close the debug panel.
		/// </summary>
		public void CloseDebugPanel() {
			isPanelVisible_ = false;
			debugPanel.SetActive(false);
		}

	}
}
