using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlattenHierarchy : MonoBehaviour {

    private static Transform selectedRoot = null;
    [MenuItem("GameObject/FlattenHierarchy", priority = 0)]
    public static void Flatten() {
        
        Transform[] selectedTransforms = Selection.transforms;
        
        for (int i = 0; i < selectedTransforms.Length; i++) {
            selectedRoot = selectedTransforms[i].parent;
            FlattenChildren(selectedTransforms[i]);
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
