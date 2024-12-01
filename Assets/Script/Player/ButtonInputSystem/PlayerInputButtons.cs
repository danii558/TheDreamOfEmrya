using UnityEngine;
using static UnityEngine.Input;

[CreateAssetMenu(fileName = "ButtonsData", menuName = "Player/PlayerInputButtons")]
public class PlayerInputButtons : ScriptableObject, IButtonsInput
{
	public KeyCode forward;
	public KeyCode backward;
	public KeyCode rightward;
	public KeyCode leftward;

	[Space]
	public KeyCode run;
	public KeyCode jump;


	public KeyCode Forward { get { return forward; } }
	public KeyCode Backward { get { return backward; } }
	public KeyCode Rightward { get { return rightward; } }
	public KeyCode Leftward { get { return leftward; } }

	public KeyCode Run { get { return run; } }
	public KeyCode Jump { get { return jump; } }

	public bool IsButtonPressed(KeyCode keyCode, PushMode hold)
	{
		return hold switch
		{
			PushMode.Press => GetKeyDown(keyCode),
			PushMode.Hold => GetKey(keyCode),
			PushMode.Release => GetKeyUp(keyCode),
			_ => false
		};
	}
}