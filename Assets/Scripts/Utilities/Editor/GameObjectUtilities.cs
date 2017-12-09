using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectUtilities : MonoBehaviour {

    private static Transform selectedRoot = null;
    [MenuItem("GameObject/Utilities/(Dangerous)/FlattenHierarchy", priority = 0)]
    public static void FlattenHierarchy() {

        Transform[] selectedTransforms = Selection.transforms;

        for (int i = 0; i < selectedTransforms.Length; i++) {
            selectedRoot = selectedTransforms[i].parent;
            FlattenChildren(selectedTransforms[i]);
        }
    }

    [MenuItem("GameObject/Utilities/(Dangerous)/RemoveAllColliders", priority = 0)]
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
