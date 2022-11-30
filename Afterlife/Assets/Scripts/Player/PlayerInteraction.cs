using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Implements player look-based interaction.
/// </summary>
public class PlayerInteraction : PlayerDelegate
{
	/// <summary>
	/// Player camera transform reference.
	/// </summary>
	[SerializeField, Tooltip("Player camera transform reference.")]
	private Transform PlayerLook = null;

	/// <summary>
	/// Player inspection position.
	/// </summary>
	[SerializeField, Tooltip("Player inspection position.")]
	private Transform PlayerInspect = null;

	/// <summary>
	/// Interaction sphere cast length.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Interaction sphere cast length.")]
	private float InteractRange = 8f;

	/// <summary>
	/// Interaction sphere cast radius.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Interaction sphere cast radius.")]
	private float InteractRadius = 0.5f;

	/// <summary>
	/// Current tracked player listener.
	/// </summary>
	private IPlayerListener Listener = null;

	/// <summary>
	/// Should an interaction occur this frame?
	/// </summary>
	private bool DoInteract = false;

	/// <summary>
	/// Should interaction be ignored?
	/// </summary>
	private bool BlockInteract = false;

	void Start()
	{
		if (!PlayerLook)
		{
			Debug.LogError("[PLAYER INTERACTION] - NO CAMERA TRANSFORM LINKED; INTERACTION DISABLED.");
		}

		if (!PlayerInspect)
		{
			Debug.LogError("[PLAYER INTERACTION] - NO INSPECTION TRANSFORM LINKED; INTERACTION DISABLED.");
		}
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{
		if (context.action.name.Equals("Interact"))
		{
			DoInteract = context.performed && !BlockInteract;
			BlockInteract = BlockInteract && !(BlockInteract && context.canceled);
		}
	}

	void FixedUpdate()
	{
		if (!PlayerLook || !PlayerInspect)
		{
			return;
		}

		foreach (RaycastHit hit in Physics.SphereCastAll(PlayerLook.position, InteractRadius, PlayerLook.forward, InteractRange))
		{
			IPlayerListener component = hit.transform.GetComponent<IPlayerListener>();

			if (component != null)
			{
				Listener = component;
				return;
			}
		}

		Listener = null;
	}

	public override void UpdateDelegate(PlayerContext context)
	{
		if (!PlayerLook || !PlayerInspect)
		{
			return;
		}

		context.InspectTransform = PlayerInspect;

		if (Listener != null)
		{
			Listener.OnHover(context);

			if (DoInteract && !BlockInteract)
			{
				Listener.OnInteraction(context);

				DoInteract = false;
				BlockInteract = true;
			}
		}
	}
}