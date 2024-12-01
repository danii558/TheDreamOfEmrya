using System;
using UnityEngine;
using static ErrorUtils;
using static MovementUtils;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour, IUpdatableController
{
	[SerializeField] private MonoBehaviour inputHandler;
	[SerializeField] private MonoBehaviour raycastChecker;
	[SerializeField] private Transform tripod;

	[Header("Settings")]
	[SerializeField, Range(0, 20)] private float walkSpeed = 3f;
	[SerializeField, Range(0, 40)] private float runSpeed = 6f;
	[SerializeField, Range(0, 10)] private float stopSpeed = 1f;
	[Space]
	[SerializeField, Range(1, 20)] private float rotationSmoothnessSpeed = 10f;
	[Space]
	[SerializeField, Range(0, 10)] private float jumpHeight = 5;
	[SerializeField, Range(0, 10)] private float doubleJumpHeight = 3;

	private IPlayerControlInput playerControlInput;
	private IRaycastChecker iRaycastChecker;
	private Rigidbody rb;

	private float jumpVelocity = 0;
	private float doubleJumpVelocity = 0;

	private bool useDoubleJump = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();

		if (inputHandler == null) { NullError(this, nameof(inputHandler)); }
		if (raycastChecker == null) { NullError(this, nameof(raycastChecker)); }
		if (tripod == null) { NullError(this, nameof(tripod)); }
	}

	private void OnValidate()
	{
		CheckInterfaceError(ref playerControlInput, ref inputHandler);
		CheckInterfaceError(ref iRaycastChecker, ref raycastChecker);

		jumpVelocity = CalculateJumpVelocity(jumpHeight);
		doubleJumpVelocity = CalculateJumpVelocity(doubleJumpHeight);

	}

	public void UpdateControl()
	{
		Vector3 force   = playerControlInput.GetMovementInput();
		bool jump       = playerControlInput.IsJumpingInput();
		int walkingMode = playerControlInput.WalkingMode;

		bool isFrontColliding = iRaycastChecker.CheckRaycastFront();
		bool isLegColliding   = iRaycastChecker.CheckRaycastLeg();

		if (force != Vector3.zero && !isFrontColliding) { SetPosition(force, walkingMode); }
		if (walkingMode > 0) { SetRotation(force); }

		if (jump)
		{
			if (isLegColliding)
			{
				Jump(jumpVelocity);
				useDoubleJump = false;
			}
			else if (!useDoubleJump)
			{
				Jump(doubleJumpVelocity);
				useDoubleJump = true;
			}
		}
	}

	/// <summary>
	/// Executes a jump by setting the vertical velocity of the object.
	/// </summary>
	/// <param name="velocity">The vertical velocity that will be applied to the object for the jump.</param>
	private void Jump(float velocity)
	{
		SetVelocity(rb, y: velocity);
	}

	/// <summary>
	/// Sets the position of the object based on the applied force and walking mode.
	/// </summary>
	/// <param name="force">The force to apply to the object.</param>
	/// <param name="walkingMode">The current walking mode (0 - Stop, 1 - Walk, 2 - Run).</param>
	private void SetPosition(Vector3 force, int walkingMode)
	{
		float speed = walkingMode switch
		{
			2 => runSpeed,
			1 => walkSpeed,
			0 => stopSpeed,
			_ => 0,
		};

		force = RecalculateForce(force, tripod, speed);

		SetVelocity(rb, x: force.x, z: force.z);
	}

	/// <summary>
	/// Updates the rotation of the rigidbody to face the direction of the input force, taking into account the tripod's current Y rotation.
	/// </summary>
	/// <param name="force">The force vector that determines the target direction for rotation.</param>
	private void SetRotation(Vector3 force)
	{
		Quaternion curentTargetRotation = CalculateTargetRotation(force, tripod.eulerAngles.y);

		SetTargetRotation(rb, curentTargetRotation, rotationSmoothnessSpeed);
	}
}

public static class MovementUtils
{
	/// <summary>
	/// Sets the velocity of a Rigidbody component, allowing selective modification of each axis (X, Y, Z).
	/// If any axis value is not provided, the current velocity for that axis will be retained.
	/// </summary>
	/// <param name="rb">The Rigidbody component to apply the velocity change to.</param>
	/// <param name="x">Optional. The new velocity value for the X axis. If null, the current X velocity is retained.</param>
	/// <param name="y">Optional. The new velocity value for the Y axis. If null, the current Y velocity is retained.</param>
	/// <param name="z">Optional. The new velocity value for the Z axis. If null, the current Z velocity is retained.</param>
	public static void SetVelocity(Rigidbody rb, float? x = null, float? y = null, float? z = null)
	{
		float newX = x ?? rb.velocity.x;
		float newY = y ?? rb.velocity.y;
		float newZ = z ?? rb.velocity.z;

		rb.velocity = new Vector3(newX, newY, newZ);
	}

	/// <summary>
	/// Smoothly interpolates the rotation of the rigidbody towards the target rotation using a specified smoothness factor.
	/// </summary>
	/// <param name="rb">The rigidbody whose rotation will be updated.</param>
	/// <param name="targetRotation">The target rotation that the rigidbody will gradually rotate towards.</param>
	/// <param name="rotationSmoothnessSpeed">The speed factor determining how smoothly the rotation transitions to the target rotation.</param>
	public static void SetTargetRotation(Rigidbody rb, Quaternion targetRotation, float rotationSmoothnessSpeed)
	{
		rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSmoothnessSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Calibrates the given force vector by adjusting its direction according to the provided transform
	/// and applies the specified speed. The Y-axis is set to 0 to constrain movement to the horizontal plane.
	/// </summary>
	/// <param name="force">The original force vector to be recalibrated.</param>
	/// <param name="direction">The transform whose direction will be applied to the force vector.</param>
	/// <param name="speed">The speed factor that will be applied to the resulting force vector.</param>
	/// <returns>A normalized force vector recalibrated based on the transform's direction and adjusted with the given speed.</returns>
	public static Vector3 RecalculateForce(Vector3 force, Transform direction, float speed)
	{
		force = direction.TransformDirection(force);

		return new Vector3(force.x, 0, force.z).normalized * speed;
	}

	/// <summary>
	/// Calculates the target rotation based on the direction of the applied force and additional rotation offset.
	/// </summary>
	/// <param name="force">The direction of the applied force, which determines the forward direction of the rotation.</param>
	/// <param name="rotationOffset">An additional Y-axis rotation offset to be applied to the target rotation.</param>
	/// <returns>Returns a quaternion representing the final calculated rotation on the Y-axis.</returns>
	public static Quaternion CalculateTargetRotation(Vector3 force, float rotationOffset)
	{
		Quaternion targetRotation = Quaternion.LookRotation(force);
		return Quaternion.Euler(0, targetRotation.eulerAngles.y + rotationOffset, 0);
	}

	/// <summary>
	/// Calculates the initial jump velocity required to reach a specific height.
	/// </summary>
	/// <param name="jumpHeight">The height the character needs to jump (in meters).</param>
	/// <returns>The initial velocity required for the jump.</returns>
	public static float CalculateJumpVelocity(float jumpHeight)
	{
		/// The formula used is derived from kinematics: v = sqrt(2 * g * h),
		/// where g is the acceleration due to gravity and h is the jump height.
		return Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
	}
}

public interface IPlayerControlInput
{
	/// <summary>
	/// Gets the current walking mode for the character
	/// </summary>
	int WalkingMode { get; }

	/// <summary>
	/// Processes input from the player to calculate movement direction and speed.
	/// </summary>
	/// <returns>
	/// A Vector3 representing the force to apply to the player object.
	/// The force vector contains the movement direction and magnitude.
	/// The boolean flag is used to indicate whether the player is moving at full speed.
	/// </returns>
	Vector3 GetMovementInput();

	/// <summary>
	/// Checks whether the space button is pressed for jumping or other actions.
	/// </summary>
	/// <returns>
	/// Returns true if the space button is pressed.
	/// Returns false otherwise.
	/// </returns>
	bool IsJumpingInput();
}

/// <summary>
/// Interface that defines the methods for checking collisions with leg and front colliders.
/// </summary>
public interface IRaycastChecker
{
	/// <summary>
	/// Checks for a collision using a raycast directed from the leg's offset.
	/// The ray is cast in the direction and distance defined in the Leg settings.
	/// </summary>
	/// <returns>Returns true if the raycast hits an object on the specified layer for legs.</returns>
	bool CheckRaycastLeg();

	/// <summary>
	/// Checks for a collision using a raycast directed from the front's offset.
	/// The ray is cast in the direction and distance defined in the Front settings.
	/// </summary>
	/// <returns>Returns true if the raycast hits an object on the specified layer for the front.</returns>
	bool CheckRaycastFront();

}
