using Unity.VisualScripting;
using UnityEngine;
using static ErrorUtils;
using static UnityEngine.Input;

public class PlayerInputHandler : MonoBehaviour, IPlayerControlInput
{
	[SerializeField] private ScriptableObject playerInputButtons;

	private IButtonsInput buttonsInput;

	private bool isMoving = false;

	public int WalkingMode { get {  return CalculateWalkingMode(); } }

	private void Awake()
	{
		if (playerInputButtons == null) { NullError(this, nameof(playerInputButtons)); }
	}

	private void OnValidate()
	{
		CheckInterfaceError(ref buttonsInput, ref playerInputButtons);
	}

	public Vector3 GetMovementInput()
	{
		Vector3 force = Vector3.zero;

		bool fullSpeed = false;

		// Z axis (forward/backward) movement.
		force.z = GetAxisValue(buttonsInput.Forward, buttonsInput.Backward, ref fullSpeed);

		// X axis (left/right) movement.
		force.x = GetAxisValue(buttonsInput.Rightward, buttonsInput.Leftward, ref fullSpeed);

		isMoving = fullSpeed;
		return force;
	}

	public int CalculateWalkingMode()
	{
		return isMoving ? GetKey(buttonsInput.Run) ? 2 : 1 : 0;
	}

	public bool IsJumpingInput()
	{
		return GetKeyDown(buttonsInput.Jump);
	}

	/// <summary>
	/// Calculates axis value based on button input.
	/// </summary>
	/// <param name="holdPositive">Button code for holding movement in the positive direction (e.g., forward).</param>
	/// <param name="stopPositive">Button code for stopping movement in the positive direction.</param>
	/// <param name="holdNegative">Button code for holding movement in the negative direction (e.g., backward).</param>
	/// <param name="stopNegative">Button code for stopping movement in the negative direction.</param>
	/// <param name="fullSpeed">
	/// A reference to a boolean that will be set to true if the player is holding a movement key,
	/// indicating full-speed movement.
	/// </param>
	/// <returns>
	/// Returns 1 for positive direction, -1 for negative direction, and 0 for no movement.
	/// </returns>
	private float GetAxisValue(KeyCode first, KeyCode second, ref bool fullSpeed)
	{
		if (buttonsInput.IsButtonPressed(first, PushMode.Hold))
		{
			fullSpeed = true;
			return 1f;
		}
		else if (buttonsInput.IsButtonPressed(second, PushMode.Hold))
		{
			fullSpeed = true;
			return -1f;
		}
		else if (buttonsInput.IsButtonPressed(first, PushMode.Release))
		{
			return 1f;
		}
		else if (buttonsInput.IsButtonPressed(second, PushMode.Release))
		{
			return -1f;
		}

		return 0f;
	}
}

public interface IButtonsInput
{
	KeyCode Forward { get; }
	KeyCode Backward { get; }
	KeyCode Rightward { get; }
	KeyCode Leftward { get; }

	KeyCode Run { get; }
	KeyCode Jump { get; }

	bool IsButtonPressed(KeyCode keyCode, PushMode hold);
}

public enum PushMode
{
	Press,
	Hold,
	Release
}