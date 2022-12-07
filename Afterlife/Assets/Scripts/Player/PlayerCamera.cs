using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : PlayerDelegate
{
	/// <summary>
	/// Player Settings object. Used to access camera settings.
	/// </summary>
	[SerializeField, Tooltip("Player Settings object. Used to access camera settings.")]
	private PlayerSettings _settings = null;

	/// <summary>
	/// Player Transform component. Accepts horizontal rotation.
	/// </summary>
	[SerializeField, Tooltip("Player Transform component. Accepts horizontal rotation.")]
	private Transform _player = null;

	/// <summary>
	/// Camera Transform component. Accepts vertical rotation.
	/// </summary>
	[SerializeField, Tooltip("Camera Transform component. Accepts vertical rotation.")]
	private Transform _camera = null;

	/// <summary>
	/// Per-frame raw camera input vector.
	/// </summary>
	private Vector2 _input = Vector2.zero;

	/// <summary>
	/// Per-frame adjusted camera input vector.
	/// </summary>
	private Vector2 _scale = Vector2.zero;

	/// <summary>
	/// Per-frame current look vector.
	/// </summary>
	private Vector2 _look = Vector2.zero;

	/// <summary>
	/// Is the current input source normalized?
	/// </summary>
	private bool _normalized = false;

	/// <summary>
	/// Blocks inputs.
	/// </summary>
	private bool _ignoreInput = false;

	void Awake()
	{
		DelegateOrder = CAMERA_ORDER; // Update the camera first.
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (!_settings)
		{
			Debug.LogError("[PLAYER CAMERA] - NO CAMERA SETTINGS LINKED; CAMERA MOVEMENT DISABLED.");
		}

		if (!_player)
		{
			Debug.LogError("[PLAYER CAMERA] - NO PLAYER TRANSFORM FOUND; CAMERA MOVEMENT DISABLED.");
		}

		if (!_camera)
		{
			Debug.LogError("[PLAYER CAMERA] - NO CAMERA TRANSFORM FOUND; CAMERA MOVEMENT DISABLED.");
		}
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{
		if (context.action.name.Equals("Camera"))
		{
			_normalized = !(context.action.activeControl.device is Pointer);
			_input = _ignoreInput ? Vector2.zero : context.ReadValue<Vector2>();
		}
	}

	public override void UpdateDelegate(PlayerContext context)
	{
		_ignoreInput = context.Inspecting || context.Paused;

		if (_settings && _player && _camera)
		{
			// Make the scale vector using our settings object.
			_scale = new Vector2(1, _settings.InvertCameraY ? -1 : 1);
			_scale *= _normalized ? _settings.SensitivityNormal : _settings.SensitivityPointer;
			_scale *= _normalized ? Time.deltaTime : 1f;

			// Scales the input by the scale vector.
			_scale = Vector2.Scale(_input, _scale);

			// Increment.
			_look += _scale;
			_look.y = Mathf.Clamp(_look.y, -90f, 90f);

			// Rotate our tracked transforms to get the camera to actually move.
			_player.localRotation = Quaternion.AngleAxis(_look.x, _player.up);
			_camera.localRotation = Quaternion.AngleAxis(-_look.y, Vector3.right);
		}
	}
}
