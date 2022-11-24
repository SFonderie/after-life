using System;
using UnityEngine;

/// <summary>
/// Contains shared Player Object data.
/// </summary>
[Serializable]
public class PlayerContext : BehaviorContext
{
	/// <summary>
	/// Player move speed in meters per second.
	/// </summary>
	[Header("Ground Movement"), Min(0f), Tooltip("Player move speed in meters per second.")]
	public float MovementSpeed = 2.0f;

	/// <summary>
	/// How effectively ground movement responds to input.
	/// </summary>
	[Range(0f, 1f), Tooltip("How effectively ground movement responds to input.")]
	public float GroundControl = 1.0f;

	/// <summary>
	/// Player jump height in meters.
	/// </summary>
	[Header("Vertical Movement"), Min(0f), Tooltip("Player jump height in meters.")]
	public float JumpHeight = 1.0f;

	/// <summary>
	/// Player coyote time in seconds.
	/// </summary>
	[Min(0f), Tooltip("Player coyote time in seconds.")]
	public float CoyoteTime = 0.2f;

	/// <summary>
	/// How effectively air movement responds to input.
	/// </summary>
	[Range(0f, 1f), Tooltip("How effectively air movement responds to input.")]
	public float AirControl = 0.05f;

	/// <summary>
	/// Per-frame player movement intent.
	/// </summary>
	public Vector3 Intent { get; set; }

	/// <summary>
	/// Per-frame player movement velocity.
	/// </summary>
	public Vector3 Velocity { get; set; }

	/// <summary>
	/// Per-frame player grounded state.
	/// </summary>
	public bool Grounded { get; set; }
}