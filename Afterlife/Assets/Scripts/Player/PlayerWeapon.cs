using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : PlayerDelegate
{
	/// <summary>
	/// Point where projectile emerge.
	/// </summary>
	[SerializeField, Tooltip("Point where projectiles emerge.")]
	private Transform Muzzle = null;

	void Start()
	{

	}

	public override void HandleInput(InputAction.CallbackContext context)
	{

	}

	public override void UpdateDelegate(PlayerContext context)
	{

	}
}
