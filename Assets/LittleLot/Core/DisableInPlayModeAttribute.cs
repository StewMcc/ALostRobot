using UnityEngine;

// @StewMcc 23/03/2018
namespace LittleLot {

	/// <summary>
	/// Disables the property when in Unity Editor play mode.
	/// <para/>
	/// Usage: [DisableInPlayMode], usually used as [SerializeField, DisableInPlayMode].
	/// </summary>
	/// <remarks>
	/// Causes it to be disabled when in Unity PlayMode, stopping it from being changed.
	///  Useful for values used in initialisation or when changing the value won't do anything.
	/// </remarks>
	public class DisableInPlayModeAttribute : PropertyAttribute {

	}
}
