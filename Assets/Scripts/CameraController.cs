using UnityEngine;

/// <summary>
/// Controls the movement of the camera folliwing the Robot.
/// Also moves it to the distress beacon when the game is completed.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
	[SerializeField]
	private RobotController robot =null;

	[Tooltip("Distance camera is offset from the bot.")]
	[SerializeField]
	private Vector3 cameraOffset = Vector3.zero;

	[SerializeField]
	private float transitionTime = 3;

	private float startTime_ =0;

	private Vector3 initialPosition_ = Vector3.zero;

	private Vector3 lerpStartPosition_ = Vector3.zero;

	private bool isFollowingRobot_ = true;

	private bool isMovingToDistressBeacon_ = true;

	private void Start() {
		initialPosition_ = gameObject.transform.position;
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

			transform.position = new Vector3(
				robot.transform.position.x + cameraOffset.x,
				transform.position.y + cameraOffset.y,
				robot.transform.position.z + cameraOffset.z);


		}
		else if (isMovingToDistressBeacon_) {
			transform.position = MathUtil.SmoothLerp(
				lerpStartPosition_,
				initialPosition_,
				startTime_,
				transitionTime,
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
