using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Automatically handles input for a given Game Object.
/// </summary>
public class InputManager : MonoBehaviour
{
	/// <summary>
	/// Player Input component reference. Used to source input events.
	/// </summary>
	[SerializeField, Tooltip("Player Input component reference. Used to source input events.")]
	private PlayerInput _input = null;

	/// <summary>
	/// List of all input listeners.
	/// </summary>
	private List<IInputListener> _listeners = new List<IInputListener>();

	private void Start()
	{
		if (_input)
		{
			_input.onActionTriggered += HandleInput;
		}

		FindListeners();
	}

	/// <summary>
	/// Passes input events down to all listeners.
	/// </summary>
	/// <param name="context">Input event context.</param>
	private void HandleInput(InputAction.CallbackContext context)
	{
		foreach (IInputListener listener in _listeners)
		{
			listener.HandleInput(context);
		}
	}

	/// <summary>
	/// Loads the listeners array with all sibling and child components.
	/// </summary>
	private void FindListeners()
	{
		_listeners.Clear();
		_listeners.AddRange(GetComponents<IInputListener>());
		_listeners.AddRange(GetComponentsInChildren<IInputListener>());
	}
}
