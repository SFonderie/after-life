using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages Player Behavior scripts.
/// </summary>
public class PlayerManager : BehaviorManager<PlayerDelegate, PlayerContext>
{
	/// <summary>
	/// Player Input component reference. Used to source input events.
	/// </summary>
	[SerializeField, Tooltip("Player Input component reference. Used to source input events.")]
	private PlayerInput _input = null;

	void Start()
	{
		if (_input)
		{
			_input.onActionTriggered += HandleInput;
		}
	}

	/// <summary>
	/// Passes input events to all Player Delegates.
	/// </summary>
	/// <param name="context">Input event context struct.</param>
	public void HandleInput(InputAction.CallbackContext context)
	{
		foreach (PlayerDelegate script in Delegates)
		{
			script.HandleInput(context);
		}
	}
}
