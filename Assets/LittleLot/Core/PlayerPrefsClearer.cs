using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LittleLot {

	/// <summary>
	/// Utility class for cleaning up all PlayerPrefs.
	/// </summary>
	public class PlayerPrefsClearer : MonoBehaviour {

		/// <summary>
		/// Deletes all of the PlayerPrefs currently set.
		/// </summary>
		/// <remarks>
		/// Uses PlayerPrefs.DeleteAll(), and pops up a warning dialogue when in the UnityEditor.
		/// </remarks>
		public void DangerousDeleteAllPlayerPreferencesMenu() {
			DeleteAllPlayerPreferences();
		}

#if UNITY_EDITOR
		[MenuItem("Edit/Delete all player preferences")]
#endif
		private static void DeleteAllPlayerPreferences() {
#if UNITY_EDITOR
			if (EditorUtility.DisplayDialog("Delete All Player Preferences?",
					"Are you sure you want to destroy ALL! of the player preferences?",
					"Delete All", "Cancel")) {
				PlayerPrefs.DeleteAll();
			}
#else
		PlayerPrefs.DeleteAll();
#endif
		}
	}
}
