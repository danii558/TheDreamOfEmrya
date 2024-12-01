using UnityEngine;
using static RaycastUtils;

public class CheckRaycast : MonoBehaviour, IRaycastChecker
{
	[Header("Leg settings")]
	[SerializeField] private Vector3 offsetLeg;
	[SerializeField] private Vector3 directionDistanceLeg;
	[SerializeField] private LayerMask layerMaskLeg;

	[Header("Front settings")]
	[SerializeField] private Vector3 offsetFront;
	[SerializeField] private Vector3 directionDistanceFront;
	[SerializeField] private float interval;
	[SerializeField] private uint quantity;
	[SerializeField] private LayerMask layerMaskFront;

	private const float remainderOfQuantity = 1;

	private void OnValidate()
	{
		if (interval < 0) interval = 0;
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		MyDrawLine(transform, offsetLeg, directionDistanceLeg);

		Gizmos.color = Color.blue;
		for (int i = 0; i < quantity; i++)
		{
			MyDrawLine(transform, offsetFront + i * (interval / (quantity - remainderOfQuantity)) * Vector3.up, directionDistanceFront);
		}
	}

	public bool CheckRaycastLeg()
	{
		return MyRaycast(transform, offsetLeg, directionDistanceLeg, layerMaskLeg);
	}

	public bool CheckRaycastFront()
	{
		for (int i = 0; i < quantity; i++)
		{
			if (MyRaycast(transform, offsetFront + i * (interval / (quantity - remainderOfQuantity)) * Vector3.up, directionDistanceFront, layerMaskFront))
				return true;
		}
		return false;
	}
}

public static class RaycastUtils
{
	/// <summary>
	/// Casts a ray from a specified offset relative to the transform in the given direction, checking for collisions.
	/// </summary>
	/// <param name="transform">The transform from which the ray will be cast.</param>
	/// <param name="offset">The local space offset from the transform's position where the ray will start.</param>
	/// <param name="directionDistance">The direction and distance of the ray in local space. The magnitude of this vector determines the ray's length.</param>
	/// <param name="layerMask">The layer mask that filters what the ray can collide with.</param>
	/// <returns>Returns true if the ray hits a collider, false otherwise.</returns>
	public static bool MyRaycast(Transform transform, Vector3 offset, Vector3 directionDistance, LayerMask layerMask)
	{
		return Physics.Raycast(transform.position + transform.TransformDirection(offset),
						transform.TransformDirection(directionDistance).normalized,
						directionDistance.magnitude,
						layerMask);
	}

	/// <summary>
	/// Draws a line in the scene view from a specified offset relative to the transform in the given direction.
	/// This is primarily for visualizing rays or directions in the Unity editor.
	/// </summary>
	/// <param name="transform">The transform from which the line will be drawn.</param>
	/// <param name="offset">The local space offset from the transform's position where the line will start.</param>
	/// <param name="directionDistance">The direction and length of the line in local space.</param>
	public static void MyDrawLine(Transform transform, Vector3 offset, Vector3 directionDistance)
	{
		Gizmos.DrawLine(transform.position + transform.TransformDirection(offset),
				transform.position + transform.TransformDirection(offset) + transform.TransformDirection(directionDistance));
	}

	/// <summary>
	/// Calculates an offset based on the provided index, quantity, interval, and remainder of quantity.
	/// The result moves the offset upwards depending on the index and interval.
	/// </summary>
	/// <param name="offset">The initial local offset position.</param>
	/// <param name="index">The current index in a loop or sequence that determines how much the offset is modified.</param>
	/// <param name="quantity">The total number of divisions or segments used in the calculation.</param>
	/// <param name="interval">The distance between each offset step.</param>
	/// <param name="remainderOfQuantity">A value that adjusts the final quantity calculation (used to avoid dividing by zero or other special cases).</param>
	/// <returns>Returns the calculated offset as a new `Vector3`, shifted upwards based on the index and interval.</returns>
	public static Vector3 CalculatedOffSet(Vector3 offset, int index, int quantity, float interval, float remainderOfQuantity)
	{
		return offset + index * (interval / (quantity - remainderOfQuantity)) * Vector3.up;
	}
}