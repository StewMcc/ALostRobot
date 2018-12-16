using UnityEngine;

// @StewMcc 17/02/2018
namespace LittleLot {

	/// <summary>
	/// Shows the property with the Unity Inspector UI disabled.
	/// <para/>
	/// Usage: [ReadOnly], usually used as [SerializeField, ReadOnly].
	/// </summary>
	/// <remarks>
	/// Does not disable size adjustment for arrays or properly show custom properties.
	/// </remarks>
	public class ReadOnlyAttribute : PropertyAttribute {
	}
}
