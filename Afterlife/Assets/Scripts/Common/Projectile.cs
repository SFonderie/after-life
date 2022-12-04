using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField, Tooltip("Projectile physics handler.")]
	private Rigidbody ProjectilePhysics = null;
	private float Damage = 0;

	public void OnSpawn(Vector3 velocity, float damage)
	{
		ProjectilePhysics.velocity = velocity;
		Damage = damage;
	}

	public void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);
	}
}
