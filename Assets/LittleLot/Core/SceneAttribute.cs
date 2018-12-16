using UnityEngine;

// @StewMcc 10/12/2017
namespace LittleLot {

	/// <summary>
	/// Shows the Scene and a Scene selection window instead of the unchecked string input box.
	/// <para/>
	/// Usage: [Scene], usually used as [SerializeField, Scene]
	/// </summary>
	/// <remarks>
	/// This property drawer will also check if the selected scene has already been added to 'Scenes in the Build'
	/// if not it will warn the user.
	/// Set strings to "" for the property drawer to correctly select 'none' without display error logs.
	/// </remarks>
	public class SceneAttribute : PropertyAttribute {
	}
}