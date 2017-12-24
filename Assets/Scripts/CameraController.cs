using UnityEngine;

/// <summary>
/// Controls the movement of the camera folliwing the Robot.
/// Also moves it to the distress beacon when the game is completed.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
	[SerializeField]
	private RobotController robot = null;

	[SerializeField]
	private float followSpeed = 2.0f;

	[Tooltip("Distance camera is offset from the bot.")]
	[SerializeField]
	private Vector3 cameraOffset = Vector3.zero;

	[SerializeField]
	private float logoTransitionTime = 3;

	private float startTime_ = 0;

	private Vector3 initialPosition_ = Vector3.zero;

	private Vector3 lerpStartPosition_ = Vector3.zero;

	private Vector3 cameraVelocity_ = Vector3.zero;

	private bool isFollowingRobot_ = true;

	private bool isMovingToDistressBeacon_ = true;

	private void Start() {
		initialPosition_ = gameObject.transform.position;
		transform.position = new Vector3(
			robot.transform.position.x + cameraOffset.x,
			transform.position.y + cameraOffset.y,
			robot.transform.position.z + cameraOffset.z);
	}

	private void OnEnable() {
		EventManager.OnShipFixed += StartMovingToDistressBeacon;
	}

	private void OnDisable() {
		EventManager.OnShipFixed -= StartMovingToDistressBeacon;
	}

	/// <summary>
	/// Just before rendering, updates the camera to follow the robot.
	/// </summary>
	private void LateUpdate() {
		if (isFollowingRobot_) {

			// lerped animation between 2 points.
			transform.position = Vector3.SmoothDamp(transform.position,
				new Vector3(
				robot.transform.position.x + cameraOffset.x,
				transform.position.y + cameraOffset.y,
				robot.transform.position.z + cameraOffset.z),
				ref cameraVelocity_,
				followSpeed
			);
		} else if (isMovingToDistressBeacon_) {
			transform.position = MathUtil.SmoothLerp(
				lerpStartPosition_,
				initialPosition_,
				startTime_,
				logoTransitionTime,
				out isMovingToDistressBeacon_);

			if (!isMovingToDistressBeacon_) {
				EventManager.CameraInPosition();
			}
		}
	}

	/// <summary>
	/// Start the camera moving towards the distress beacon at its initial position.
	/// When the ship has been fixed .
	/// </summary>
	private void StartMovingToDistressBeacon() {
		isFollowingRobot_ = false;
		isMovingToDistressBeacon_ = true;

		startTime_ = Time.time;
		lerpStartPosition_ = gameObject.transform.position;
	}
}
