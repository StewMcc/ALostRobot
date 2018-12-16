using System;

using UnityEditor;

using UnityEngine;

// @StewMcc 17/02/2018
namespace LittleLot {

	/// <summary>
	/// Custom drawer that shows the Enum as a mask field instead of a singular Enum.
	/// <para/>
	/// See Also: <seealso cref="EnumFlagAttribute"/>
	/// </summary>
	[CustomPropertyDrawer(typeof(EnumFlagAttribute))]
	public class EnumFlagDrawer : PropertyDrawer {

		/// <summary>
		/// Overrides Unity display of enums tagged with [EnumFlag].
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			// Grab the enum from the properties target.
			Enum targetEnum = (Enum)fieldInfo.GetValue(property.serializedObject.targetObject);

			// Start property rendering
			EditorGUI.BeginProperty(position, label, property);
			// Use Unity's Flags field to render the enum instead of the default mask.
			// This allows you to select multiple options.
			Enum enumNew = EditorGUI.EnumFlagsField(position, label, targetEnum);
			// Convert back to an int value to represent the mask fields, as that is how its stored on the serialized property.
			property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
			// Finish property rendering
			EditorGUI.EndProperty();

		}
	}
}