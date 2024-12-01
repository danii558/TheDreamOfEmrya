using UnityEngine;
using static ErrorUtils;

public class PlayerAnimationController : MonoBehaviour, IUpdatableController
{
	[SerializeField] private Animator animator;
	[SerializeField] private MonoBehaviour inputHandler;
	[SerializeField] private MonoBehaviour raycastChecker;

	private IPlayerControlInput playerControlInput;
	private IRaycastChecker iRaycastChecker;

	private void Awake()
	{
		if (animator == null) { NullError(this, nameof(animator)); }
		if (inputHandler == null) { NullError(this, nameof(inputHandler)); }
		if (inputHandler == null) { NullError(this, nameof(raycastChecker)); }
	}

	private void OnValidate()
	{
		CheckInterfaceError(ref playerControlInput, ref inputHandler);
		CheckInterfaceError(ref iRaycastChecker, ref raycastChecker);
	}

	public void UpdateControl()
	{
		int walkingMode = playerControlInput.WalkingMode;

		bool isFrontColliding = iRaycastChecker.CheckRaycastFront();


		SetAnimationLogic(walkingMode, isFrontColliding);
	}

	/// <summary>
	/// Updates the animator's mode based on the object's movement state.
	/// </summary>
	/// <param name="isMoving">Indicates whether the object is moving.</param>
	/// <param name="isRun">Indicates whether the object is running (true) or walking (false).</param>
	private void SetAnimationLogic(int walkingMode, bool isFrontColliding)
	{
		if (isFrontColliding)
		{
			animator.SetInteger("mode", 0);
			return;
		}

		animator.SetInteger("mode", walkingMode);
	}
}
