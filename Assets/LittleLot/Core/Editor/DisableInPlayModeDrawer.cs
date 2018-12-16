using UnityEditor;

using UnityEngine;

// @StewMcc 17/02/2018
namespace LittleLot {

	/// <summary>
	/// Custom drawer that disables the property when in Unity Editor play mode.
	/// <para/>
	/// See Also: <seealso cref="DisableInPlayModeAttribute"/>
	/// </summary>
	[CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
	public class DisableInPlayModeDrawer : PropertyDrawer {

		/// <summary>
		/// Disables Unity display of properties tagged with [DisableInPlayMode].
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			EditorGUI.PropertyField(position, property, label);
			EditorGUI.EndDisabledGroup();
		}
	}
}
