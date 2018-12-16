using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// @StewMcc 11/06/18
namespace LittleLot {

	/// <summary>
	/// Scriptable object for storing persistent information.
	/// </summary>
	/// <remarks>
	/// Useful for editor window settings, or editor scripts, as allows access to persistent object
	/// that can be accessed easily without a reference, but can be accessed only by reference in builds.
	/// This can conflict with UnityEditor.ScriptableSingleton which is undocumented.
	/// </remarks>
	public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject {

#if UNITY_EDITOR

		/// <summary>
		/// The single available instance, null if doesn't exists.
		/// <para/>
		/// This should only ever be called from within the UNITY_EDITOR.
		/// </summary>
		private static T instance_ = null;

		/// <summary>
		/// Finds an instance of the ScriptableSingleton in the project and provides a reference to it.
		/// <para/>
		/// This should only ever be called from within the UNITY_EDITOR.
		/// </summary>
		public static T Instance {
			get {
				if (!instance_) {
					// FindAssets using asset database t: tag for types
					string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
					if (guids.Length == 0) {
						Debug.LogWarning("Trying to access " + typeof(T).Name + " without scriptable object!");

						Debug.LogWarning("Creating " + typeof(T).Name + " in Assets folder, this can be moved but should not be deleted");

						instance_ = CreateInstance<T>();

						AssetDatabase.CreateAsset(instance_, "Assets/" + typeof(T).Name + ".asset");
						AssetDatabase.SaveAssets();

						EditorUtility.FocusProjectWindow();

						Selection.activeObject = instance_;
					}

					if (guids.Length > 0) {
						instance_ = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
					}

					if (guids.Length > 1) {
						Debug.LogError("Multiple instances of " + typeof(T).Name + " should only be one, returning first!");
					}
				}
				return instance_;
			}
		}
#endif
	}
}
