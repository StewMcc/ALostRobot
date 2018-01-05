using UnityEditor;
using UnityEngine;

/// <summary>
/// @StewMcc 05/12/2017
/// </summary>
namespace LittleLot {

	[CustomEditor(typeof(ZombieObjectDetector))]
	public class ZombieObjectDetectorEditor : Editor {

		private SerializedObject serializedZombieDetector_;

		private SerializedProperty loggingOptions_;

		private SerializedProperty ignoredTypeStrings_;

		private SerializedProperty typesToScanStrings_;

		private SerializedProperty logZombieKeyCode_;

		private SerializedProperty logTag_;

		private ZombieObjectDetector zombieDetector_;

		private string nameOfTypeToScanToAdd_ = "";

		private bool hasFailedToAddTypeToScan_ = false;

		private string nameOfIgnoredTypeToAdd_ = "";

		private bool hasFailedToAddIgnoredType_ = false;

		private void OnEnable() {

			serializedZombieDetector_ = new SerializedObject(target);

			zombieDetector_ = (ZombieObjectDetector)(target);

			loggingOptions_ = serializedZombieDetector_.FindProperty("loggingOptions");

			logTag_ = serializedZombieDetector_.FindProperty("logTag");

			logZombieKeyCode_ = serializedZombieDetector_.FindProperty("logZombieKeyCode");

			ignoredTypeStrings_ = serializedZombieDetector_.FindProperty("ignoredTypeStrings");

			typesToScanStrings_ = serializedZombieDetector_.FindProperty("typesToScanStrings");
		}

		public override void OnInspectorGUI() {
			serializedZombieDetector_.Update();

			loggingOptions_.intValue = (int)(ZombieObjectDetector.LoggingOptions)EditorGUILayout.EnumMaskField("Logging Options", (ZombieObjectDetector.LoggingOptions)loggingOptions_.intValue);

			EditorGUILayout.PropertyField(logTag_);

			DisplayTypeList("Types to scan. Empty to scan all types.", typesToScanStrings_, ref nameOfTypeToScanToAdd_, ref hasFailedToAddTypeToScan_);

			DisplayTypeList("Types to ignore.", ignoredTypeStrings_, ref nameOfIgnoredTypeToAdd_, ref hasFailedToAddIgnoredType_);

			EditorGUI.BeginDisabledGroup(!Application.isPlaying || zombieDetector_.IsLogging());
			if (Application.isPlaying) {
				if (GUILayout.Button("Log Zombies")) {
					zombieDetector_.RunZombieObjectDetection();
				}
			} else {
				GUILayout.Button("Log Zombies (Available during Play)");
			}

			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(logZombieKeyCode_, new GUIContent("Zombie Logging Key Code", "Used for logging in builds"));


			serializedZombieDetector_.ApplyModifiedProperties();
		}

		private void DisplayTypeList(string label, SerializedProperty property, ref string inputText, ref bool hasFailedToAdd) {
			if (hasFailedToAdd && inputText == "") {
				hasFailedToAdd = false;
			}
			EditorGUILayout.BeginVertical("Box");

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(label);

			if (hasFailedToAdd) {
				Color oldColor = GUI.color;
				GUI.color = Color.red;
				EditorGUILayout.LabelField("Type Not Found: " + inputText);
				GUI.color = oldColor;
			}
			EditorGUILayout.EndHorizontal();

			// Box for adding Ignored Types.
			EditorGUILayout.BeginHorizontal("Box");
			inputText = EditorGUILayout.TextField(inputText);
			if (GUILayout.Button("+", GUILayout.Width(40.0f))) {
				// attempt add type.
				GUI.FocusControl(null);
				if (TypeHelper.IsType(inputText)) {
					AddType(inputText, property);
					inputText = "";
					hasFailedToAdd = false;
				} else {
					hasFailedToAdd = true;
				}

			}

			EditorGUILayout.EndHorizontal();

			for (int i = 0; i < property.arraySize; i++) {

				EditorGUILayout.BeginHorizontal("Box");

				EditorGUILayout.LabelField(GetType(i, property));
				if (GUILayout.Button("-", GUILayout.Width(40.0f))) {
					RemoveType(i, property);
					GUI.FocusControl(null);
				}
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
		}
		private void AddType(string typeName, SerializedProperty property) {
			property.InsertArrayElementAtIndex(property.arraySize);
			property.GetArrayElementAtIndex(property.arraySize - 1).stringValue = typeName;
		}

		private void RemoveType(int index, SerializedProperty property) {
			for (int i = index; i < (property.arraySize - 1); i++) {
				SetType(i, GetType(i + 1, property), property);
			}
			property.arraySize--;
		}

		private void SetType(int index, string typeName, SerializedProperty property) {
			property.GetArrayElementAtIndex(index).stringValue = typeName;
		}

		private string GetType(int index, SerializedProperty property) {
			return property.GetArrayElementAtIndex(index).stringValue;
		}

		[MenuItem("GameObject/LittleLot/Create Zombie Object Detector", priority = 0)]
		public static void CreateZombieDetector() {
			ZombieObjectDetector zombieDetector_ = FindObjectOfType<ZombieObjectDetector>();
			if (zombieDetector_ == null) {
				Debug.Log("Creating A Default Zombie Object Detector");
				new GameObject("ZombieObjectDetector", typeof(ZombieObjectDetector));
			} else {
				Debug.LogWarning("Zombie Object Detector Already Exists.");
			}
		}
	}

}
