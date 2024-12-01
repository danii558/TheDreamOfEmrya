using UnityEngine;

public class CursorLock : MonoBehaviour
{
	[SerializeField] private CursorLockMode cursorLockEnable = CursorLockMode.Locked;
	[SerializeField] private CursorLockMode cursorLockDisable = CursorLockMode.None;

	private void OnEnable()
	{
		Cursor.lockState = cursorLockEnable;
	}

	private void OnDisable()
	{
		Cursor.lockState = cursorLockDisable;
	}
}
