using UnityEditor;

// @StewMcc 08/12/2018
namespace LittleLot {

	/// <summary>
	/// Expose extra functionality in the editor for the Unity Editor Settings.
	/// </summary>
	/// <remarks>
	/// Adds ability to force re-serialization, by going to binary and back to text.
	/// </remarks>
	public class EditorSettingsExtensions {
		/// <summary>
		/// Changes to force binary and back to force text to cause a re-serialization of all the assets.
		/// </summary>
		[MenuItem("Tools/LittleLot/ForceReSerialization")]
		public static void ForceReSerialization() {
			// Force to binary so has to re-serialize everything.
			EditorSettings.serializationMode = SerializationMode.ForceBinary;
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			// Change back to text, and it will regenerate from the binary with any
			// upgrades /changes to the API/Variable names enforced.
			EditorSettings.serializationMode = SerializationMode.ForceText;
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}
