using UnityEngine;

/// <summary>
/// Simple Timer that allows for simple tracking of timer state.
/// Handles alot of the annoying multi variable stuff associated with simple timers.
/// Note: not incredibly accurate.
/// </summary>
class SimpleTimer {
	private float timeRemaining_ = 0.0f;

	private float duration_ = 0.0f;

	private bool isFinished_ = false;

	/// <summary>
	/// Wether the timer has finished or not.
	/// </summary>
	/// <returns> True if the timer has finished. </returns>
	public bool IsFinished() {
		return isFinished_;
	}

	/// <summary>
	/// Sets the duration of the timer.
	/// NOTE: does not reset the timer.
	/// </summary>
	/// <param name="duration"> Time in seconds for the timer to last. </param>
	public void SetTimer(float duration) {
		duration_ = duration;
	}

	/// <summary>
	/// Resets the timer to its duration and starts the timer.
	/// </summary>
	public void StartTimer() {
		isFinished_ = false;
		timeRemaining_ = duration_;
	}

	/// <summary>
	/// The amount of time remaining until the timer is finished.
	/// </summary>
	/// <returns> Time in seconds remaining on timer. </returns>
	public float TimeRemaining() {
		return timeRemaining_;
	}

	/// <summary>
	/// Current duration setting of the timer.
	/// NOTE: Look at TimeRemaining for the time remaining, this is the current duration 
	/// setting the timer will reset to when StartTimer is called.
	/// </summary>
	/// <returns> The timers initial duration. </returns>
	public float Duration() {
		return duration_;
	}

	/// <summary>
	/// Increments the timer with delta time.
	/// Will detect decide when the timer is finished.
	/// NOTE: this needs to be called for the timer to count down.
	/// </summary>
	public void Update() {
		timeRemaining_ -= Time.deltaTime;
		if (timeRemaining_ <= 0) {
			isFinished_ = true;
		}
	}

}