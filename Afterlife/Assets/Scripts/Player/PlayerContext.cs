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
	/// Time in seconds it takes to draw a weapon.
	/// </summary>
	[Header("Weapon"), Min(0), Tooltip("Time in seconds it takes to draw a weapon.")]
	public float WeaponDrawTime = 1f;

	/// <summary>
	/// Time in seconds it takes to charge the weapon.
	/// </summary>
	[Min(0), Tooltip("Time in seconds it takes to charge the weapon.")]
	public float WeaponChargeTime = 1f;

	/// <summary>
	/// Minimum time in seconds between discharges.
	/// </summary>
	[Min(0), Tooltip("Minimum time in seconds between discharges.")]
	public float WeaponFireDelay = 0.2f;

	/// <summary>
	/// Projectile speed in units per second.
	/// </summary>
	[Min(0), Tooltip("Projectile speed in units per second.")]
	public float ProjectileSpeed = 40f;

	/// <summary>
	/// Projectile damage.
	/// </summary>
	[Min(10), Tooltip("Projectile damage.")]
	public float ProjectileDamage = 50f;

	/// <summary>
	/// Is the player currently armed?
	/// </summary>
	[Tooltip("Should the player start with their weapon out?")]
	public bool Armed = true;

	/// <summary>
	/// Pointer to the player's inspection transform.
	/// </summary>
	public Transform PickupTransform { get; set; }

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

	/// <summary>
	/// Per-frame interaction state. Blocks movement.
	/// </summary>
	public bool Interacting { get; set; }

	/// <summary>
	/// Per-frame inspection state. Blocks looking.
	/// </summary>
	public bool Inspecting { get; set; }

	/// <summary>
	/// Is the player paused right now?
	/// </summary>
	public bool Paused { get; set; }

	/// <summary>
	/// Trigger the weapon pickup?
	/// </summary>
	public bool DoWeaponPickup { get; set; }

	/// <summary>
	/// Trigger the weapon pickup?
	/// </summary>
	public bool IsFadeOut { get; set; }

	/// <summary>
	/// Trigger the weapon pickup?
	/// </summary>
	public bool IsFadeLevelMax { get; set; }

	/// <summary>
	/// Current monologue text line.
	/// </summary>
	public string Monologue { get; set; }

	/// <summary>
	/// Current dialogue text line.
	/// </summary>
	public string Dialogue { get; set; }

	/// <summary>
	/// Time at which the monologue will expire.
	/// </summary>
	public float MonologueTime { get; set; }

	/// <summary>
	/// Time at which the dialogue will expire.
	/// </summary>
	public float DialogueTime { get; set; }

	/// <summary>
	/// Current player tape status. Increases or decreases.
	/// </summary>
	public int TapeState { get; set; }

	/// <summary>
	/// Is the player dead?
	/// </summary>
	public bool Dead { get; set; }

	public override void Update()
	{
		Damage -= Time.deltaTime * 3;
		Damage = Mathf.Clamp(Damage, 0, Health);
	}
}