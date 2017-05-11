using UnityEngine;

public class MathUtil : MonoBehaviour {

	/// <summary>
	/// Smoothly Lerps between start and end positions within transition time.
	/// Uses mathf smoothstep to ensure 0 - 1 translation.
	/// </summary>
	/// <param name="startPosition"> Point at which Moving should start. </param>
	/// <param name="endPosition"> Destination point. </param>
	/// <param name="startTime"> Initial time it started at. </param>
	/// <param name="transitionTime"> The maximum amount of time it should take to complete. </param>
	/// <param name="isMoving"> Whether it is still moving to the point, false if reached end point. </param>
	/// <returns> New position between points. </returns>
	public static Vector3 SmoothLerp(Vector3 startPosition, Vector3 endPosition, float startTime, float transitionTime, out bool isMoving) {
		isMoving = true;

		// calculate current percentage between 2 points.
		float timeSinceStarted = Time.time - startTime;
		float percentageComplete = timeSinceStarted / transitionTime;

		//When we've completed the lerp, we set isMoving to false
		if (percentageComplete >= 1.0f) {
			isMoving = false;
		}

		// lerped animation between 2 points.
		return Vector3.Lerp(startPosition, endPosition,
			Mathf.SmoothStep(0f, 1f, percentageComplete));
	}

	/// <summary>
	/// Smoothly Lerps between start and end positions within transition time, and then move back to start position.
	/// Uses Mathf.PingPong to allow back and forth.
	/// Uses Mathf.Smoothstep to ensure 0 - 1 translation.
	/// </summary>
	/// <param name="startPosition"> Point at which Moving should start. </param>
	/// <param name="endPosition"> Destination point. </param>
	/// <param name="startTime"> Initial time it started at. </param>
	/// <param name="transitionTime"> The maximum amount of time it should take to complete. </param>		
	/// <returns> New position between points. </returns>
	public static Vector3 SmoothPingPongLerp(Vector3 startPosition, Vector3 endPosition, float transitionTime) {

		// lerped animation between 2 points.
		return Vector3.Lerp(startPosition, endPosition,
			Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.time / transitionTime, 1f)));
	}

}

