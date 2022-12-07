using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerWeapon : PlayerDelegate
{
	[SerializeField, Tooltip("Projectile prefab type.")]
	private GameObject ProjectilePrefab = null;

	[SerializeField, Tooltip("Player weapon grip point.")]
	private Transform WeaponGrip = null;

	[Header("Weapon References"), SerializeField, Tooltip("Weapon model object.")]
	private GameObject WeaponModel = null;

	[SerializeField, Tooltip("Point where projectiles emerge.")]
	private Transform Muzzle = null;

	[SerializeField, Tooltip("Left Shield Pivot.")]
	private Transform LeftPivot = null;

	[SerializeField, Tooltip("Right Shield Pivot.")]
	private Transform RightPivot = null;

	[SerializeField, Tooltip("Fin Shield Pivot.")]
	private Transform FinPivot = null;

	[SerializeField, Tooltip("Recoil position.")]
	private Transform Recoil = null;

	/// <summary>
	/// Animation curve for weapon drawing.
	/// </summary>
	[Header("Variables"), SerializeField, Tooltip("Animation curve for weapon drawing.")]
	private AnimationCurve DrawCurve = null;

	/// <summary>
	/// Weapon draw animation value.
	/// </summary>
	private float Interpolate = 1;

	/// <summary>
	/// Current charge state.
	/// </summary>
	private float Charge = 0;

	/// <summary>
	/// Time when the player last fired the weapon.
	/// </summary>
	private float FireTime = 0;

	/// <summary>
	/// Should the player attack this frame?
	/// </summary>
	private bool DoAttack = false;

	/// <summary>
	/// Should attacks be blocked this frame?
	/// </summary>
	private bool BlockAttack = false;

	/// <summary>
	/// Can the player attack this frame?
	/// </summary>
	private bool CanAttack = false;

	/// <summary>
	/// Ignore attack inputs?
	/// </summary>
	private bool IgnoreInput = false;

	void Start()
	{
		// Hide the weapon model.
		WeaponModel.SetActive(false);
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{
		if (context.action.name.Equals("Attack"))
		{
			DoAttack = context.performed && !BlockAttack && !IgnoreInput;
			BlockAttack = BlockAttack && !(BlockAttack && context.canceled);
		}
	}

	public override void UpdateDelegate(PlayerContext context)
	{
		IgnoreInput = context.Interacting || context.Paused;

		// Draw the weapon if applicable.
		if (context.DoWeaponPickup)
		{
			context.DoWeaponPickup = false;
			context.Armed = true;
			Interpolate = 0;
		}

		WeaponModel.SetActive(context.Armed);

		if (context.Armed && WeaponModel.transform.parent != WeaponGrip)
		{
			WeaponModel.transform.SetParent(WeaponGrip, false);
		}

		float increment = Time.deltaTime / context.WeaponDrawTime;
		Interpolate += context.Armed ? increment : -increment;
		Interpolate = Mathf.Clamp(Interpolate, 0, 1);

		float angle = Mathf.LerpUnclamped(-45, 0, DrawCurve.Evaluate(Interpolate));

		// Rotation-based animation uses the weapon pivot as a base.
		WeaponGrip.parent.localRotation = Quaternion.Euler(0, 0, angle);

		LeftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -10, Charge));
		RightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 10, Charge));
		FinPivot.localRotation = Quaternion.Euler(0, Mathf.Lerp(0, 10, Charge), 0);

		Recoil.localPosition = SMath.RecursiveLerp(Recoil.localPosition, Vector3.zero, 0.1f, 2 * Time.deltaTime);

		Vector3 Kick = WeaponModel.transform.localPosition;
		Kick = SMath.RecursiveLerp(Kick, Recoil.localPosition, 0.1f, 2 * Time.deltaTime);
		WeaponModel.transform.localPosition = Kick;

		// Actually handle weapon updates.
		if (context.Armed && Interpolate == 1)
		{
			WeaponUpdate(context);
		}
	}

	private void WeaponUpdate(PlayerContext context)
	{
		CanAttack = Time.time >= FireTime + context.WeaponFireDelay;

		if (DoAttack && CanAttack)
		{
			Charge += Time.deltaTime / context.WeaponChargeTime;

			if (Charge >= 1)
			{
				// Automatic discharge.
				DischargeWeapon(context);
			}
		}
		else
		{
			Charge -= 2 * Time.deltaTime / context.WeaponFireDelay;
		}

		// Clamp the charge to keep it normal.
		Charge = Mathf.Clamp(Charge, 0, 1);
	}

	private void DischargeWeapon(PlayerContext context)
	{
		// Spawn the projectile and calculate a desired velocity for it.
		GameObject projectile = Instantiate(ProjectilePrefab, Muzzle.position, Muzzle.rotation);
		Vector3 velocity = Muzzle.forward * context.ProjectileSpeed;// + context.Velocity;

		// Actually apply the velocity and damage to the projectile.
		Projectile script = projectile.GetComponent<Projectile>();
		script.OnSpawn(velocity, context.ProjectileDamage, "Player");

		DoAttack = false;
		BlockAttack = true;
		FireTime = Time.time;

		Recoil.localPosition = new Vector3(-1, 0, 0);
	}
}
