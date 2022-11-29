using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : PlayerDelegate
{
	/// <summary>
	/// Player camera transform reference.
	/// </summary>
	[SerializeField, Tooltip("Player camera transform reference.")]
	private Transform PlayerLook = null;

	/// <summary>
	/// Interaction sphere cast length.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Interaction sphere cast length.")]
	private float InteractRange = 10f;

	/// <summary>
	/// Interaction sphere cast radius.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Interaction sphere cast radius.")]
	private float InteractRadius = 1f;

	void Start()
	{
		if (!PlayerLook)
		{
			Debug.LogError("[PLAYER INTERACTION] - NO CAMERA TRANSFORM LINKED; INTERACTION DISABLED.");
		}
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{

	}

	public override void UpdateDelegate(PlayerContext context)
	{
		
	}
}
