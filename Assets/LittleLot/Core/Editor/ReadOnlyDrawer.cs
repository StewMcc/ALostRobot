using UnityEditor;

using UnityEngine;

// @StewMcc 17/02/2018
namespace LittleLot {

	/// <summary>
	/// Custom drawer that shows there value with the GUI disabled.
	/// <para/>
	/// See Also: <seealso cref="ReadOnlyAttribute"/>
	/// </summary>
	/// <remarks>
	/// For an Arrays workaround/hack see: https://forum.unity.com/threads/prevent-built-in-array-resizing-in-inspector.397953/
	/// Custom property drawers workaround: can extend your custom property drawer to take in a bool readonly that determines its display method.
	/// More Info see Fogbugz case: 980877, IssueTracker: https://issuetracker.unity3d.com/issues/custom-property-drawers-are-not-allways-called-when-calling-property-field
	/// </remarks>
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer {

		/// <summary>
		/// Overrides Unity display of properties tagged with [ReadOnly].
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.PropertyField(position, property, label, true);
			EditorGUI.EndDisabledGroup();
		}
	}
}
