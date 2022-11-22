using UnityEngine.InputSystem;

/// <summary>
/// Allows player scripts to handle input events.
/// </summary>
public interface IInputListener
{
	/// <summary>
	/// Invoked once per input event per frame.
	/// </summary>
	/// <param name="context">Input event context.</param>
	public void HandleInput(InputAction.CallbackContext context);
}
