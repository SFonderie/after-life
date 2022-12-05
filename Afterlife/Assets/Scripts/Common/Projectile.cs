using UnityEngine;

public class Projectile : MonoBehaviour
{
	/// <summary>
	/// Projectile physics handler.
	/// </summary>
	[SerializeField, Tooltip("Projectile physics handler.")]
	private Rigidbody ProjectilePhysics = null;

	/// <summary>
	/// Projectile mesh renderer.
	/// </summary>
	[SerializeField, Tooltip("Projectile mesh renderer.")]
	private MeshRenderer ProjectileMesh = null;

	/// <summary>
	/// Projectile glow component.
	/// </summary>
	[SerializeField, Tooltip("Projectile glow component.")]
	private Light ProjectileGlow = null;

	/// <summary>
	/// Projectile trail renderer.
	/// </summary>
	[SerializeField, Tooltip("Projectile trail renderer.")]
	private TrailRenderer ProjectileTrail = null;

	/// <summary>
	/// Projectile damage amount.
	/// </summary>
	private float Damage = 0;

	/// <summary>
	/// Projectile life time.
	/// </summary>
	private float Lifetime = 0;

	/// <summary>
	/// Time at which the projectile will die.
	/// </summary>
	private float DeathTime = 10;

	/// <summary>
	/// Saved projectile glow intensity.
	/// </summary>
	private float Intensity = 0;

	public void OnSpawn(Vector3 velocity, float damage)
	{
		ProjectilePhysics.velocity = velocity;
		Intensity = ProjectileGlow.intensity;
		Damage = damage;
		DeathTime = 10;
		Lifetime = 0;
	}

	void Update()
	{
		Lifetime += Time.deltaTime;

		// Only interpolate over the last second of the projectile life.
		float interpolate = Mathf.Clamp(1 - DeathTime + Lifetime, 0, 1);
		ProjectileGlow.intensity = Mathf.Lerp(Intensity, 0, interpolate);

		if (Lifetime >= DeathTime)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.SendMessageUpwards("TakeDamage", Damage, SendMessageOptions.DontRequireReceiver);
		DeathTime = Lifetime + ProjectileTrail.time;
		ProjectilePhysics.velocity = Vector3.zero;
		ProjectilePhysics.detectCollisions = false;
		ProjectileMesh.enabled = false;
	}
}
