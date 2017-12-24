using UnityEditor;
using UnityEngine;

/// <summary>
/// @StewMcc 10/12/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// GameObject Manipulation Utilities.
	/// </summary>
	public class GameObjectUtilities : MonoBehaviour {

		private static Transform selectedRoot = null;

		/// <summary>
		/// Flattens all children to parent level, can be an expensive as recursive.
		/// </summary>
		[MenuItem("GameObject/LittleLot/FlattenHierarchy", priority = 100)]
		public static void FlattenHierarchy() {

			Transform[] selectedTransforms = Selection.transforms;

			for (int i = 0; i < selectedTransforms.Length; i++) {
				selectedRoot = selectedTransforms[i].parent;
				FlattenChildren(selectedTransforms[i]);
				UnityEditor.EditorUtility.DisplayProgressBar("FlattenHierarchy",
						" Flattening: " + selectedRoot.name,
						i / selectedTransforms.Length);
			}
		}

		/// <summary>
		/// Removes All the mesh colliders from the selected object and its children.
		/// </summary>
		[MenuItem("GameObject/LittleLot/RemoveAllMeshColliders", priority = 100)]
		public static void RemoveAllColliders() {
			GameObject[] selectedGameObjects = Selection.gameObjects;
			MeshCollider collider = null;
			for (int i = 0; i < selectedGameObjects.Length; i++) {
				collider = null;
				collider = selectedGameObjects[i].GetComponent<MeshCollider>();
				if (collider) {
					DestroyImmediate(collider);
				}
			}
		}

		private static void FlattenChildren(Transform parent) {

			Transform child;
			while (parent.childCount > 0) {
				child = parent.GetChild(0);
				child.SetParent(selectedRoot, true);

				child.name = string.Format("{0}_{1}", parent.name, child.name);

				FlattenChildren(child);
			}
		}
	}
}
