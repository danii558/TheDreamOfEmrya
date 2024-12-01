using UnityEngine;

public class TripodMovementController : MonoBehaviour, IUpdatableController
{
	[SerializeField] private Transform tripod;

	[Header("Settings")]
	[SerializeField] private Vector3 tripodOffset;
	[SerializeField, Range(0, 1000)] private float sensitivity = 200f;
	[SerializeField, Range(-90, 90)] private float clampedVerticalRotation = 45f;

	[SerializeField] private bool horizontalInvert = false;
	[SerializeField] private bool verticalInvert = false;

	private Vector2 rotation;

	private void Awake()
	{
		if (tripod == null) { ErrorUtils.NullError(this, nameof(tripod)); }
    }

    public void UpdateControl()
	{
		SetPosition();
		SetRotation();
	}

	/// <summary>
	/// Reads and applies mouse input to rotate the camera based on sensitivity and inversion settings.
	/// </summary>
	private void SetRotation()
	{
		float horizontalInput = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		float verticalInput   = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

		// Invert
		horizontalInput = horizontalInvert ? horizontalInput : -horizontalInput;
		verticalInput   = verticalInvert   ? verticalInput   : -verticalInput;

		rotation.x += horizontalInput;
		rotation.y = Mathf.Clamp(rotation.y + verticalInput, -clampedVerticalRotation, clampedVerticalRotation);

		tripod.localRotation = Quaternion.Euler(rotation.y, rotation.x, 0);
	}

	/// <summary>
	/// Updates the position of the tripod based on the current position and offset.
	/// </summary>
	private void SetPosition()
	{
		tripod.position = transform.position + tripodOffset;
	}
}