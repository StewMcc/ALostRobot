using UnityEditor;

using UnityEngine;

// @StewMcc 10/12/2017
namespace LittleLot {

	/// <summary>
	/// Custom drawer that shows the selected scene and a scene selection menu instead of strings.
	/// <para/>
	/// See Also: <seealso cref="SceneAttribute"/>
	/// </summary>
	[CustomPropertyDrawer(typeof(SceneAttribute))]
	public class SceneDrawer : PropertyDrawer {

		/// <summary>
		/// Overrides Unity's display of strings tagged with [Scene] to display a Unity Scene.
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			// Only override rendering on properties with type of string (otherwise it doesn't work).
			if (property.propertyType == SerializedPropertyType.String) {
				// Get currently selected object (generated from previous string value).
				SceneAsset sceneObject = GetSceneObject(property.stringValue, property);
				// Render a SceneAsset object field, with the previous string value, shown as a SceneAsset.
				Object scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);

				if (scene == null) {
					// We get nulls if the scene asset isn't found so wipe the string value.
					property.stringValue = "";

					// If the scene name doesn't match the property they have changed it
					// either by drag n drop or the object picker.
				} else if (scene.name != property.stringValue) {
					// Convert the Object to a SceneAsset.
					SceneAsset sceneObj = GetSceneObject(scene.name, property);
					// If its a valid scene asset we use it, otherwise assume select invalid and ignore.
					if (sceneObj != null) {
						property.stringValue = scene.name;
					}
				}
			} else {
				EditorGUI.LabelField(position, label.text, "Only use [Scene] with strings.");
			}
		}

		/// <summary>
		/// Retrieve the scene string from the editor build settings, returns null if not found.
		/// </summary>
		/// <param name="sceneObjectName"> The asset name path of the scene object. </param>
		/// <returns> The SceneAsset or null if not found. </returns>
		protected SceneAsset GetSceneObject(string sceneObjectName, SerializedProperty property) {
			if (string.IsNullOrEmpty(sceneObjectName)) {
				// Early exit as know it will return null when string null or empty.
				return null;
			}
			// Iterate over the scenes in the build settings
			foreach (EditorBuildSettingsScene editorScene in EditorBuildSettings.scenes) {
				// We found the scene object's name in the editor scene's path.
				// This assumes that a scene will not be named exactly the same as a parent folder & a duplicate of another scene.
				if (editorScene.path.IndexOf(sceneObjectName) != -1) {
					return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
				}
			}
			Debug.LogWarning(string.Format("Scene[{0}] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.{1}Set on {2}, parameter: {3}",
				sceneObjectName, System.Environment.NewLine, property.serializedObject.targetObject, property.displayName));
			return null;
		}
	}
}
