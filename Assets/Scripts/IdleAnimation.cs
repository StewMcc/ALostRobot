using UnityEngine;

/// <summary>
/// PickUp Idle Animation script, gives nice rotation and scaling animation.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class IdleAnimation : MonoBehaviour {

	[SerializeField]
	[Tooltip("Used for end rotation, position and scale values, Ensure rotation angles are positive...reasons.")]
	private Transform pingPongTarget = null;

	[Header("Animation")]
	[SerializeField]
	private bool isAnimated = true;

	[SerializeField]
	private bool isMoving = true;

	[SerializeField]
	[Tooltip("The amount of time in seconds it should take to move to the target.")]
	private float moveToTargetInterval =2.0f;

	[SerializeField]
	private bool isScaling = true;

	[SerializeField]
	[Tooltip("The amount of time in seconds it should take to scale to the targets scale.")]
	private float scaleToTargetInterval =2.0f;

	[SerializeField]
	private bool isRotating = true;

	[SerializeField]
	[Tooltip("The amount of time in seconds it should take to rotate to the targets rotation.")]
	private float rotateToTargetInterval =2.0f;

	private Vector3 pingPongMoveTo_;

	private Vector3 pingPongMoveFrom_;

	private Vector3 pingPongScaleTo_;

	private Vector3 pingPongScaleFrom_;

	private Vector3 pingPongRotateTo_;

	private Vector3 pingPongRotateFrom_;

	private bool wasAnimating_ = true;

	private float initialHeight_ = 0.0f;

	/// <summary>
	/// Store initial start and end points, and ensures tag is correctly set.
	/// </summary>
	private void Start() {
		initialHeight_ = transform.position.y;
		SetupPingPongTargets();
	}

	/// <summary>
	/// Update the animations.
	/// Moving,rotating and scaling between values.
	/// </summary>
	private void Update() {
		if (isAnimated && pingPongTarget) {
			if (isMoving) {
				PingPongMoveToTarget();
			}
			if (isRotating) {
				PingPongRotateToTarget();
			}
			if (isScaling) {
				PingPongScaleToTarget();
			}
		}
	}

	/// <summary>
	/// Stores the initial to and from targets for animations.
	/// </summary>
	private void SetupPingPongTargets() {
		pingPongMoveTo_ = pingPongTarget.position;
		pingPongMoveFrom_ = transform.position;

		pingPongScaleTo_ = pingPongTarget.localScale;
		pingPongScaleFrom_ = transform.localScale;

		pingPongRotateTo_ = pingPongTarget.rotation.eulerAngles;
		pingPongRotateFrom_ = transform.rotation.eulerAngles;
	}

	/// <summary>
	/// Pauses the animation effects.
	/// </summary>
	public void PauseAnimation() {
		wasAnimating_ = isAnimated;
		isAnimated = false;
	}

	/// <summary>
	/// Resets the height of the object and restarts the animation.
	/// </summary>
	public void ResumeAnimation() {
		isAnimated = wasAnimating_;
		Vector3 newPosition = transform.position;
		// reset the height of the object to its initial height.
		transform.position = new Vector3(newPosition.x, initialHeight_, newPosition.z);
		// Update move to and from
		pingPongMoveTo_ = pingPongTarget.position;
		pingPongMoveFrom_ = transform.position;
	}

	/// <summary>
	/// Moves the collectable towards the set target with lerp(smoothstep(pingpong))).
	/// Gives a nice dampened animation effect back and forth.
	/// </summary>
	private void PingPongMoveToTarget() {
		transform.position = MathUtil.SmoothPingPongLerp(pingPongMoveFrom_, pingPongMoveTo_, moveToTargetInterval);
	}

	/// <summary>
	/// Rotates the collectable towards the set target with lerp(smoothstep(pingpong))).
	/// Gives a nice dampened animation effect back and forth.
	/// </summary>
	private void PingPongRotateToTarget() {
		transform.rotation = Quaternion.Euler(MathUtil.SmoothPingPongLerp(pingPongRotateFrom_, pingPongRotateTo_, rotateToTargetInterval));
	}

	/// <summary>
	/// Scales the collectable towards the set target with lerp(smoothstep(pingpong))).
	/// Gives a nice dampened animation effect back and forth.
	/// </summary>
	private void PingPongScaleToTarget() {
		transform.localScale = MathUtil.SmoothPingPongLerp(pingPongScaleFrom_, pingPongScaleTo_, scaleToTargetInterval);
	}

}
