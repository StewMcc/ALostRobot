using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// @StewMcc 05/12/2017
/// </summary>
namespace LittleLot {

	public class ZombieObjectDetector : MonoBehaviour {
		const BindingFlags kAllStaticFields =
			BindingFlags.NonPublic |        // Include protected and private
			BindingFlags.Public |           // Also include public 
			BindingFlags.FlattenHierarchy | // Include parent members
			BindingFlags.Static;            // Specify to retrieve static

		const BindingFlags kAllFields =
			BindingFlags.NonPublic |        // Include protected and private
			BindingFlags.Public |           // Also include public 
			BindingFlags.FlattenHierarchy | // Include parent members
			BindingFlags.Static |           // Specify to retrieve static
			BindingFlags.Instance;          // Include instance members

		const BindingFlags kAllNonStaticFields =
			BindingFlags.NonPublic |        // Include protected and private
			BindingFlags.Public |           // Also include public 
			BindingFlags.FlattenHierarchy | // Include parent members
			BindingFlags.Instance;          // Include instance members

		[Flags]
		public enum LoggingOptions {
			kIgnoredMembers = 1 << 0,                    // Logs each member that is ignored by the zombie detector.
			kFieldInfoNotFound = 1 << 1,                 // Reflection failed to find the FieldInfo for the connected object.
			kMemberType = 1 << 2,                        // Logs each Members Type.
			kFieldType = 1 << 3,                         // Logs each Member of type Field's type.
			kAlreadyScanned = 1 << 4,                    // Logs Objects that are ignored as they have already been scanned previously.
			kInvalidType = 1 << 5,                       // Logs information about type that are ignroed because they are invalid for causing zombie objects (int,float etc)
			kExceptions = 1 << 6,                        // kExceptions that are generally ignored as logged more succinctly else where.
			kListBadEqualsImplementations = 1 << 7,      // Lists all Bad .Equals Implementations that were found causing the object and all its members to be ignored.
			kListScannedObjects = 1 << 8,                // Lists all objects that have had there members checked for zombie objects.
			kListScannedStaticMembers = 1 << 9,          // List of all Static Members that have been checked from there roots.
			kZombieCountForEachStaticField = 1 << 10,    // Individual Zombie Object count for Static field roots.
			kZombieStackTrace = 1 << 11                  // Default zombie object stack trace, used to find root to zombie object causes of leaks.
		}

		[SerializeField]
		LoggingOptions loggingOptions = LoggingOptions.kZombieStackTrace | LoggingOptions.kZombieCountForEachStaticField;

		[SerializeField]
		string logTag = "";

		[SerializeField]
		string[] ignoredTypeStrings = new string[0];

		[SerializeField]
		string[] typesToScanStrings = new string[0];

		[SerializeField]
		KeyCode logZombieKeyCode = KeyCode.None;

		bool isLogging_ = false;

		Stack<MemberInfo> memberInfoChain_ = new Stack<MemberInfo>();

		Stack<MemberInfo> scannedStaticMembers_ = new Stack<MemberInfo>();

		Stack<object> scannedObjects_ = new Stack<object>();

		List<Type> invalidTypes_ = new List<Type>();

		float progress_ = 0.0f;

		float progressIncrements_ = 0.0f;

		object testObject_ = new object();

		int zombieObjectCount_ = 0;

		int totalZombieObjectCount_ = 0;

		int totalMembersLookedAt_ = 0;

		StreamWriter fileOutputStream_;

		string logFileFolder_;

		public bool IsLogging() {
			return isLogging_;
		}

		public void RunZombieObjectDetection() {
			if (!isLogging_) {
				StartCoroutine(LogZombies());
			} else {
				Debug.LogWarning("ZombieDetector Already Logging!");
			}
		}

		private void Start() {
			logFileFolder_ = Path.Combine(Application.dataPath, ".ZombieLogs");
		}
		private void Update() {
			if (Input.GetKeyDown(logZombieKeyCode)) {
				RunZombieObjectDetection();
			}
		}

		private void OnGUI() {
			if (isLogging_) {
				GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Logging Zombie Objects!");
			}
		}

		private IEnumerator LogZombies() {
			isLogging_ = true;
			// fake wait for really fast logs, so indicator can be seen.
			yield return new WaitForSeconds(0.5f);

			ProcessAllObjectsFromStaticRoots();

			isLogging_ = false;

			yield return null;
		}

		private void ProcessAllObjectsFromStaticRoots() {
			// Just to clean up stack traces from debug.Log so easier to read.
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
			Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);

			if (loggingOptions == 0) {
				Debug.LogWarning("No Logging Options Set");
				return;
			}
			CreateFile();

			memberInfoChain_.Clear();
			scannedObjects_.Clear();
			scannedStaticMembers_.Clear();
			invalidTypes_.Clear();

			totalZombieObjectCount_ = 0;
			totalMembersLookedAt_ = 0;

			Log("Logging Zombie Objects");
			Log(string.Format("Logging Options: {0}{1}IgnoredTypes: {2}{1}ScannedTypes: {3}",
				loggingOptions.ToString(),
				Environment.NewLine,
				string.Join(", ", ignoredTypeStrings),
				string.Join(", ", typesToScanStrings)
				));

			Assembly assembly = Assembly.GetExecutingAssembly();
			Type[] types = assembly.GetTypes();

			progressIncrements_ = 1.0f / types.Length;
			progress_ = 0.0f;

			foreach (Type type in types) {
				progress_ += progressIncrements_;
				// only care about classes and structs, and valid types.
				if (IsValidZombieType(type)) {

					if (typesToScanStrings.Length > 0) {
						// skip types not in includedInitialTypes
						if (!typesToScanStrings.Contains(type.FullName)) {
							continue;
						}
					}
#if UNITY_EDITOR
					UnityEditor.EditorUtility.DisplayProgressBar("ZombieDetector",
						" Searching: " + type.FullName.ToString(),
						progress_);
#endif

					memberInfoChain_.Clear();
					TraverseStaticMembersFromType(type);
				}
			}
			if (HasLoggingOptions(LoggingOptions.kListBadEqualsImplementations)) {
				if (invalidTypes_.Count > 0) {
					Log(string.Format("Bad Implementation of .Equals In Types: {0}{1}{2}",
						invalidTypes_.Count.ToString(),
						Environment.NewLine,
						string.Join(Environment.NewLine,
						invalidTypes_.Select(x => x.ToString()).Reverse().ToArray())));
				}
			}

			if (HasLoggingOptions(LoggingOptions.kListScannedObjects)) {
				if (scannedObjects_.Count > 0) {
					Log(string.Format("Scanned Objects: {0}{1}{2}",
						scannedObjects_.Count.ToString(),
						Environment.NewLine,
						string.Join(Environment.NewLine,
						scannedObjects_.Select(x => x.GetType().ToString()).Reverse().ToArray())));
				}
			}

			if (HasLoggingOptions(LoggingOptions.kListScannedStaticMembers)) {
				if (scannedStaticMembers_.Count > 0) {
					Log(string.Format("Scanned Static Members: {0}{1}{2}",
						scannedStaticMembers_.Count.ToString(),
						Environment.NewLine,
						FormatStackOfMemberInfo(scannedStaticMembers_)));
				}
			}

#if UNITY_EDITOR
			UnityEditor.EditorUtility.ClearProgressBar();
#endif

			Log(string.Format("Scan Complete: Zombie Objects Found:{1}{0}Total Scanned Objects:{2}, Total Scanned Static Members:{3}, Total Members:{4}",
				Environment.NewLine,
				totalZombieObjectCount_.ToString(),
				scannedObjects_.Count.ToString(),
				scannedStaticMembers_.Count.ToString(),
				totalMembersLookedAt_.ToString()));

			CloseFile();


			// Reset the logging type.
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
			Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
		}

		private void TraverseStaticMembersFromType(Type type) {

			List<MemberInfo> memberInfos = type.GetMembers(kAllStaticFields).ToList();

			foreach (MemberInfo memberInfo in memberInfos) {
				totalMembersLookedAt_++;
				zombieObjectCount_ = 0;
				memberInfoChain_.Push(memberInfo);
				switch (memberInfo.MemberType) {
					case MemberTypes.Event:
						if (HasLoggingOptions(LoggingOptions.kMemberType)) {
							Log("Static Event: " + FormatMemberInfo(memberInfo));
						}
						scannedStaticMembers_.Push(memberInfo);
						TraverseMemberEvent(memberInfo, null);
						break;
					case MemberTypes.Field:
						if (HasLoggingOptions(LoggingOptions.kMemberType)) {
							Log("Static Field: " + FormatMemberInfo(memberInfo));
						}
						scannedStaticMembers_.Push(memberInfo);
						TraverseMemberField(memberInfo, null);
						break;
					default:
						if (HasLoggingOptions(LoggingOptions.kIgnoredMembers)) {
							Log("Static Member Ignored: " + memberInfo.MemberType);
						}
						break;
				}
				memberInfoChain_.Pop();

				if (HasLoggingOptions(LoggingOptions.kZombieCountForEachStaticField)) {
					if (zombieObjectCount_ > 0) {
						LogWarning(string.Format("Number of ZombieObjects: {0}{1}{2}",
							zombieObjectCount_, Environment.NewLine, FormatMemberInfo(memberInfo)));
					}
				}
			}
		}

		private void TraverseMemberEvent(MemberInfo memberInfo, object memberParentObject) {
			FieldInfo fieldInfo = memberInfo.ReflectedType.GetField(memberInfo.Name, kAllFields);

			if (fieldInfo != null) {
				if (IsValidZombieType(fieldInfo.FieldType)) {

					MulticastDelegate eventMulticastDelegate = fieldInfo.GetValue(memberParentObject) as MulticastDelegate;
					if (eventMulticastDelegate != null) {

						Delegate[] delegates = eventMulticastDelegate.GetInvocationList();
						foreach (Delegate eventDelegate in delegates) {
							object delegateObject = eventDelegate.Target;
							if (delegateObject != null) {
								// Search all of the delegateObjects members.
								TraverseAllMembersFromObject(delegateObject);
							}
						}
					}
				}
			} else {
				if (HasLoggingOptions(LoggingOptions.kFieldInfoNotFound)) {
					Log(string.Format("Failed to find FieldInfo: Type: {0} Member: {1}",
						memberInfo.ReflectedType,
						memberInfo.Name));
				}
			}
		}

		private void TraverseMemberField(MemberInfo memberInfo, object memberParentObject) {
			FieldInfo fieldInfo = memberInfo.ReflectedType.GetField(memberInfo.Name, kAllFields);

			if (fieldInfo != null) {

				if (IsValidZombieType(fieldInfo.FieldType)) {

					if (fieldInfo.FieldType.IsArray) {
						if (HasLoggingOptions(LoggingOptions.kFieldType)) {
							Log("Array: " + FormatMemberInfo(memberInfo));
						}

						IEnumerable collection = fieldInfo.GetValue(memberParentObject) as IEnumerable;
						if (collection != null) {
							foreach (object collectionObject in collection) {
								// if it is refrencing something
								if (collectionObject != null) {
									// search all of its members
									TraverseAllMembersFromObject(collectionObject);
								}
							}
						}
					} else { // try to get the value of everything else.

						try {
							if (HasLoggingOptions(LoggingOptions.kFieldType)) {
								Log("Other: " + FormatMemberInfo(memberInfo));
							}
							object fieldObj = fieldInfo.GetValue(memberParentObject) as object;

							// if it is refrencing something
							if (fieldObj != null) {
								// search all of its members
								TraverseAllMembersFromObject(fieldObj);
							}
						} catch (Exception e) {
							if (HasLoggingOptions(LoggingOptions.kExceptions)) {
								Debug.LogErrorFormat("Error: {0}{1}{2}",
									FormatMemberInfo(memberInfo),
									Environment.NewLine,
									e);
							}
						}
					}
				} else {
					if (HasLoggingOptions(LoggingOptions.kFieldInfoNotFound)) {
						Log(string.Format("Failed to find FieldInfo: Type: {0} Member: {1}",
							memberInfo.ReflectedType,
							memberInfo.Name));
					}
				}
			}
		}

		private void TraverseAllMembersFromObject(object obj) {

			if (IsValidZombieType(obj.GetType())) {
				try {
					// Checks if .Equals has been implemented properly before adding to the object list.
					obj.Equals(testObject_);

					if (scannedObjects_.Contains(obj)) {
						if (HasLoggingOptions(LoggingOptions.kAlreadyScanned)) {
							Log("Already Scanned: " + obj.GetType());
						}
						// Still add it to scanned objects       
						scannedObjects_.Push(obj);
						return;
					}
				} catch (Exception e) {

					if (HasLoggingOptions(LoggingOptions.kExceptions)) {
						Debug.LogErrorFormat("Error In Objects .Equals: {0}{1}{2}",
							obj.GetType(),
							Environment.NewLine,
							e);
					}
					if (!invalidTypes_.Contains(obj.GetType())) {
						invalidTypes_.Add(obj.GetType());
					}
					return;
				}

				scannedObjects_.Push(obj);

				LogIsZombieObject(obj);

				List<MemberInfo> memberInfos = obj.GetType().GetMembers(kAllNonStaticFields).ToList();

				foreach (MemberInfo memberInfo in memberInfos) {
					totalMembersLookedAt_++;
					memberInfoChain_.Push(memberInfo);
					switch (memberInfo.MemberType) {
						case MemberTypes.Event:
							if (HasLoggingOptions(LoggingOptions.kMemberType)) {
								Log("Event: " + FormatMemberInfo(memberInfo));
							}
							TraverseMemberEvent(memberInfo, obj);
							break;
						case MemberTypes.Field:
							if (HasLoggingOptions(LoggingOptions.kMemberType)) {
								Log("Field: " + FormatMemberInfo(memberInfo));
							}
							TraverseMemberField(memberInfo, obj);
							break;
						default:
							if (HasLoggingOptions(LoggingOptions.kIgnoredMembers)) {
								Log("Member Ignored: " + memberInfo.MemberType);
							}
							break;
					}
					memberInfoChain_.Pop();
				}
			}

		}

		/// <summary>
		/// Uses Unity Fake Null
		/// https://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
		/// https://forum.unity.com/threads/fun-with-null.148090/
		/// ToString results in "null" when unity has passed the object to garbage collector.
		/// </summary>
		private void LogIsZombieObject(object obj) {
			if (obj.ToString() == "null") {
				//if (obj.Equals(null)) {
				totalZombieObjectCount_++;
				zombieObjectCount_++;
				if (HasLoggingOptions(LoggingOptions.kZombieStackTrace)) {
					// Ouput the Zombie Object type, as obj.ToString() => "null" due to unity wiping the data.
					LogWarning(string.Format("ZombiedObject: {0}{1}{2}",
						obj.GetType(),
						Environment.NewLine,
						FormatStackOfMemberInfo(memberInfoChain_)));
				}
			}
		}

		private string FormatStackOfMemberInfo(Stack<MemberInfo> memberInfoList) {
			return string.Join(Environment.NewLine, memberInfoList.Select(x => FormatMemberInfo(x)).ToArray());
		}

		private string FormatMemberInfo(MemberInfo memberInfo) {
			return string.Format("->Class: {0}: Member: {1}", memberInfo.ReflectedType, memberInfo.ToString());
		}

		private bool IsValidZombieType(Type type) {
			bool result = false;

			if (!TypeHelper.IsZombieType(type)) {
				if (HasLoggingOptions(LoggingOptions.kInvalidType)) {
					Log(string.Format("kInvalidType: Basic value types: {0}", type));
				}
				// shouldnt handle basic value types (int,float etc):/
			} else if (invalidTypes_.Contains(type)) {
				if (HasLoggingOptions(LoggingOptions.kInvalidType)) {
					Log(string.Format("kInvalidType: Already found: {0}", type));
				}
			} else if (type.IsPointer) {
				if (HasLoggingOptions(LoggingOptions.kInvalidType)) {
					Log(string.Format("kInvalidType: C# style pointer: {0}", type));
				}
				// shouldn't try to handle unsafe c# style pointers.
			} else if (type.ContainsGenericParameters || type.IsGenericParameter || type.IsGenericTypeDefinition) {
				if (HasLoggingOptions(LoggingOptions.kInvalidType)) {
					Log(string.Format("InvalidFieldType: GenericParamType: {0}{1}{2}",
						type,
						Environment.NewLine,
						"Ex Singleton<T> : MonoBehaviour where T: MonoBehaviour"));
				}
				// Doesn't currently handle Ex Singleton<T> : MonoBehaviour where T : MonoBehaviour: nicely. 
				// but all derived classes will get scanned, including parent members.
			} else if (ignoredTypeStrings.Contains(type.FullName)) {
				if (HasLoggingOptions(LoggingOptions.kInvalidType)) {
					Log(string.Format("kInvalidType: User Defined Ignore: {0}", type));
				}
			} else {
				result = true;
			}
			return result;
		}

		private void CreateFile() {
			try {
				Directory.CreateDirectory(logFileFolder_);

				fileOutputStream_ = File.CreateText(Path.Combine(logFileFolder_,
					string.Format("{0}_{1}_{2}{3}", "ZombieDetector",
					DateTime.UtcNow.ToString("yyyy-dd-M--HH-mm-ss"),
					logTag,
					".log")));
			} catch (Exception) {
				Debug.LogWarning(string.Format("Failed to write to path:{0}{1}This is due to a write permision failure. i.e Console Packaged Builds.{1}If on Console try a Push-Build.",
					logFileFolder_, Environment.NewLine));
				fileOutputStream_ = null;
			}

		}

		private void CloseFile() {
			if (fileOutputStream_ != null) {
				fileOutputStream_.Close();
				Debug.Log("ZombieLog Saved to: " + logFileFolder_);
			} else {
				Debug.Log("ZombieLog Failed to Save to: " + logFileFolder_);
			}
		}

		private bool HasLoggingOptions(LoggingOptions loggingOptionsToCheckFor) {
			return ((loggingOptions & loggingOptionsToCheckFor) != 0);
		}

		private void Log(string outputText) {
			Debug.Log(outputText);
			if (fileOutputStream_ != null) {
				fileOutputStream_.WriteLine(Environment.NewLine + outputText);
			}
		}

		private void LogWarning(string outputText) {
			Debug.LogWarning(outputText);
			if (fileOutputStream_ != null) {
				fileOutputStream_.WriteLine(Environment.NewLine + outputText);
			}
		}
	}
}
