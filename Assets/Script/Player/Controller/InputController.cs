using UnityEngine;

public class InputController : MonoBehaviour
{
	[SerializeField] private MonoBehaviour[] controllables;

	private void Update()
	{
		foreach (var controllable in controllables)
		{
			if (controllable is IUpdatableController controllableObject)
			{
				controllableObject.UpdateControl();
			}
		}
	}
}

public interface IUpdatableController
{
	/// <summary>
	/// Defines the core control logic for the implementing class. 
	/// This method is called every frame to handle updates related to player or object control.
	/// </summary>
	void UpdateControl();
}

public static class ErrorUtils
{
	/// <summary>
	/// Logs a null reference error and disables the provided MonoBehaviour component.
	/// </summary>
	/// <param name="component">The MonoBehaviour component in which the error occurred.</param>
	/// <param name="name">The name of the object or field that is null.</param>
	public static void NullError(MonoBehaviour component, string name)
	{
		Debug.LogError($"Null Reference Exceptions: {name} in {component.GetType().Name} Component\n{name} is None", component);
		component.enabled = false;
	}

	public static void CheckInterfaceError(ref IButtonsInput buttonsInput, ref ScriptableObject scriptable)
	{
		if (scriptable == null) { return; }

		buttonsInput = scriptable as IButtonsInput;
		if (buttonsInput == null)
		{
			scriptable = null;
		}
	}

	public static void CheckInterfaceError(ref IPlayerControlInput playerControl, ref MonoBehaviour monoBehaviour)
	{
		if (monoBehaviour == null) { return; }

		playerControl = monoBehaviour as IPlayerControlInput;
		if (playerControl == null)
		{
			monoBehaviour = null;
		}
	}

	public static void CheckInterfaceError(ref IRaycastChecker checkCollider, ref MonoBehaviour monoBehaviour)
	{
		if (monoBehaviour == null) { return; }

		checkCollider = monoBehaviour as IRaycastChecker;
		if (checkCollider == null)
		{
			monoBehaviour = null;
		}
	}
}