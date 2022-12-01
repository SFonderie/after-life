using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerDelegate
{
	/// <summary>
	/// Character Controller component. Used to actually handle movement.
	/// </summary>
	[SerializeField, Tooltip("Character Controller component. Used to actually handle movement.")]
	private CharacterController _controller = null;

	/// <summary>
	/// Coyote timer.
	/// </summary>
	private Timer _coyote = new Timer();

	/// <summary>
	/// Player movement intent.
	/// </summary>
	private Vector2 _intent = Vector2.zero;

	/// <summary>
	/// Scaled player movement intent in 3D.
	/// </summary>
	private Vector3 _update = Vector3.zero;

	/// <summary>
	/// Current player horizontal velocity in 3D.
	/// </summary>
	private Vector3 _current = Vector3.zero;

	/// <summary>
	/// Current player vertical velocity in 3D.
	/// </summary>
	private Vector3 _vertical = Vector3.zero;

	/// <summary>
	/// Tracks jump height locally to catch changes.
	/// </summary>
	private float _heightTracker = 0f;

	/// <summary>
	/// Speed needed to reach a certain height.
	/// </summary>
	private float _jumpSpeed = 0f;

	/// <summary>
	/// Flag indicating toggle intent.
	/// </summary>
	private bool _doJump = false;

	/// <summary>
	/// Flag that toggles jump intent.
	/// </summary>
	private bool _blockJumps = false;

	/// <summary>
	/// Flag that toggles input.
	/// </summary>
	private bool _ignoreInput = false;

	void Awake()
	{
		DelegateOrder = MOVEMENT_ORDER;
	}

	void Start()
	{
		if (!_controller)
		{
			Debug.LogError("[PLAYER MOVEMENT] - NO CHARACTER CONTROLLER FOUND; PLAYER MOVEMENT DISABLED.");
		}
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{
		if (context.action.name.Equals("Movement"))
		{
			_intent = _ignoreInput ? Vector2.zero : context.ReadValue<Vector2>();
		}

		if (context.action.name.Equals("Jump"))
		{
			_doJump = context.performed && !_blockJumps && !_ignoreInput;
			_blockJumps = _blockJumps && !(_blockJumps && context.canceled);
		}
	}

	public override void UpdateDelegate(PlayerContext context)
	{
		_ignoreInput = context.Interacting;

		if (_controller)
		{
			// Actually figure out where the player wants to go.
			_update = new Vector3(_intent.x, 0, _intent.y) * context.MovementSpeed;
			_update = _controller.transform.TransformDirection(_update);
			context.Intent = _update;

			// Update player velocities.
			if (context.Grounded)
			{
				_current = SMath.RecursiveLerp(_current, _update, 0.01f, 2 * Time.deltaTime * context.GroundControl);

				// Constantly reset the timer.
				_coyote.Set(context.CoyoteTime);

				// Reset basic jump info.
				_vertical = Vector3.zero;
				CheckJumpHeight(context);
			}
			else
			{
				_current = SMath.RecursiveLerp(_current, _update, 0.01f, 2 * Time.deltaTime * context.AirControl);
			}

			// Jump if the player wants to jump and can.
			if (_doJump && !_coyote.HasElapsed && !_blockJumps)
			{
				_vertical = Vector3.up * _jumpSpeed;
				_coyote.End();

				_doJump = false;
				_blockJumps = true;
			}

			// Add gravity and mix the two velocities.
			_vertical += Physics.gravity * Time.deltaTime;
			context.Velocity = _current + _vertical;

			// Finally, we can move the player for real.
			_controller.Move(context.Velocity * Time.deltaTime);
			context.Grounded = _controller.isGrounded;
		}
	}

	/// <summary>
	/// Updates the jump speed if there is a local / context mismatch.
	/// All of this is basically just to avoid a square root call. Whee.
	/// </summary>
	private void CheckJumpHeight(PlayerContext context)
	{
		if (_heightTracker != context.JumpHeight)
		{
			_jumpSpeed = Mathf.Sqrt(2 * context.JumpHeight * Physics.gravity.magnitude);
			_heightTracker = context.JumpHeight;
		}
	}
}
