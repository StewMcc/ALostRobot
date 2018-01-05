using UnityEngine;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Handles events to do with Loading System.
	/// </summary>
	public class EventManagerLoadingSystem : MonoBehaviour {

		public delegate void EventHandlerLoadingSystem();
		/// <summary>
		/// Event to listen to for when the level has finished loading.
		/// </summary>
		public static event EventHandlerLoadingSystem OnLoadingFinished;

		/// <summary>
		/// Should be called when the new level has finished loading.
		/// </summary>
		public static void FinishedLoading() {
			// notify all listeners to event.
			if (OnLoadingFinished != null) {
				OnLoadingFinished();
			}
		}

	}
}

