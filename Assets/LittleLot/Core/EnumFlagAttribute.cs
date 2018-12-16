using UnityEngine;

// @StewMcc 17/02/2018
namespace LittleLot {

	/// <summary>
	/// Shows the Enum mask selection instead of a singular Enum drop down box.
	/// <para/>
	/// Usage: [EnumFlag], usually used as [SerializeField, EnumFlag].
	/// </summary>
	/// <remarks>
	/// This will allow you to select: Nothing, Everything, and multiple of your enum values, to be used as bit masks.
	/// </remarks>
	public class EnumFlagAttribute : PropertyAttribute {
	}
}
