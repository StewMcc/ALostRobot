using UnityEditor;
using UnityEngine;

/// <summary>
/// @StewMcc 10/12/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Scene Drawer, strings tagged [Scene] now show Scene selection instead of strings.
	/// 
	/// Use [SerializeField, Scene] to display theScene selection window instead of a string input box.
	/// This property drawer will also check if the selected scene has already been added to 'Scenes in the Build'
	/// if not it will warn the user.
	/// Set strings to "" for the property drawer to correctly select 'none' without display error logs.
	/// </summary>
	[CustomPropertyDrawer(typeof(SceneAttribute))]
	public class SceneDrawer : PropertyDrawer {

		/// <summary>
		/// Overrides Unity dispay of scenes tagged with [Scene].
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			if (property.propertyType == SerializedPropertyType.String) {
				SceneAsset sceneObject = GetSceneObject(property.stringValue);
				UnityEngine.Object scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
				if (scene == null) {
					property.stringValue = "";
				} else if (scene.name != property.stringValue) {
					SceneAsset sceneObj = GetSceneObject(scene.name);
					if (sceneObj != null) {
						property.stringValue = scene.name;
					}
				}
			} else
				EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
		}

		/// <summary>
		/// Retrive the scene string from the editor build settings, returns null if not found.
		/// 
		/// </summary>
		/// <param name="sceneObjectName"> The asset name path of the scene object. </param>
		/// <returns> The SceneAsset or null if not found. </returns>
		protected SceneAsset GetSceneObject(string sceneObjectName) {
			if (string.IsNullOrEmpty(sceneObjectName)) {
				return null;
			}

			foreach (EditorBuildSettingsScene editorScene in EditorBuildSettings.scenes) {
				if (editorScene.path.IndexOf(sceneObjectName) != -1) {
					return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
				}
			}
			Debug.LogWarning("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
			return null;
		}
	}
}