using UnityEngine;

public class Projectile : MonoBehaviour
{
	/// <summary>
	/// Projectile physics handler.
	/// </summary>
	[SerializeField, Tooltip("Projectile physics handler.")]
	private Rigidbody ProjectilePhysics = null;

	/// <summary>
	/// Projectile damage amount.
	/// </summary>
	private float Damage = 0;

	/// <summary>
	/// Projectile life time.
	/// </summary>
	private float Lifetime = 0;

	public void OnSpawn(Vector3 velocity, float damage)
	{
		ProjectilePhysics.velocity = velocity;
		Damage = damage;
		Lifetime = 0;
	}

	void Update()
	{
		Lifetime += Time.deltaTime;

		if (Lifetime > 10)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		collision.gameObject.SendMessageUpwards("TakeDamage", Damage, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
