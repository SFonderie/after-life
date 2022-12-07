using UnityEngine;

/// <summary>
/// Actually makes the drone track the player's position.
/// </summary>
public class DroneControl : EnemyDelegate
{
	/// <summary>
	/// Transform component of the drone itself.
	/// </summary>
	[SerializeField, Tooltip("Transform component of the drone itself.")]
	private Transform DroneTransform = null;

	/// <summary>
	/// Transform component of the drone eye.
	/// </summary>
	[SerializeField, Tooltip("Transform component of the drone eye.")]
	private Transform EyeTransform = null;

	/// <summary>
	/// Pupil transforms. Fires a bolt for each when the drone attacks.
	/// </summary>
	[SerializeField, Tooltip("Pupil transforms. Fires a bolt for each when the drone attacks.")]
	private Transform[] Pupils = null;

	/// <summary>
	/// Size of the drone's awareness sphere.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Size of the drone's awareness sphere.")]
	private float VisionRadius = 10f;

	/// <summary>
	/// If true, forces the drone to ignore foes.
	/// </summary>
	[SerializeField, Tooltip("If true, forces the drone to ignore foes.")]
	public bool IgnoreEnemy = false;

	/// <summary>
	/// If true, attack even when vision is blocked.
	/// </summary>
	[SerializeField, Tooltip("If true, attack even when vision is blocked.")]
	private bool Wallhack = false;

	/// <summary>
	/// Projectile prefab.
	/// </summary>
	[Header("Attack"), SerializeField, Tooltip("Projectile prefab.")]
	private GameObject ProjectilePrefab = null;

	/// <summary>
	/// Time in seconds between drone attacks.
	/// </summary>
	[Min(1), SerializeField, Tooltip("Time in seconds between drone bursts.")]
	private float FireDelay = 2f;

	/// <summary>
	/// Time in seconds between drone attacks.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Time in seconds between drone attacks.")]
	private float BurstDelay = 0.5f;

	/// <summary>
	/// Number of projectiles fired in each burst.
	/// </summary>
	[Min(1), SerializeField, Tooltip("Number of projectiles fired in each burst.")]
	private int BurstCount = 1;

	/// <summary>
	/// Damage dealt by each projectile.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Damage dealt by each projectile.")]
	private float Damage = 10;

	/// <summary>
	/// Projectile speed.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Projectile speed.")]
	private float ProjectileSpeed = 20;

	/// <summary>
	/// Time when the drone can next attack.
	/// </summary>
	private float FireTime = 0;

	/// <summary>
	/// Current burst status.
	/// </summary>
	private int BurstOn = 0;

	/// <summary>
	/// Drone aggression level.
	/// </summary>
	private float Anger = 0;

	void Start()
	{
		if (!DroneTransform)
		{
			DroneTransform = transform;
		}

		if (FireTime < Time.time + 1)
		{
			FireTime = Time.time + 1;
		}

		Anger = 0;
	}

	public override void UpdateDelegate(EnemyContext context)
	{
		if (!IgnoreEnemy)
		{
			foreach (Collider collider in Physics.OverlapSphere(transform.position, VisionRadius))
			{
				if (collider.gameObject.tag.Equals("Player"))
				{
					if (Wallhack)
					{
						UpdateVisible(context, collider);
						break;
					}

					Vector3 direction = collider.transform.position - transform.position + Vector3.up * 0.5f;
					float magnitude = direction.magnitude;

					// Get a mask representing the bitwise NOT of the enemy and player.
					int mask = ~LayerMask.GetMask(new string[] { "Enemy", "Player" });

					if (Physics.Raycast(transform.position, direction / magnitude, magnitude, mask))
					{
						// THERE IS SOMETHING IN THE WAY!
						DelayAttack();
					}
					else
					{
						// OKAY, MURDER TIME FUN TIME.
						UpdateVisible(context, collider);
					}
				}
			}
		}
		else
		{
			DelayAttack();
		}

		Anger -= Time.deltaTime;
		Anger = Mathf.Clamp(Anger, 0, 1);
		context.Anger = Anger;
	}

	private void DelayAttack()
	{
		if (FireTime < Time.time + 1)
		{
			FireTime = Time.time + 1;
		}
	}

	private void UpdateVisible(EnemyContext context, Collider collider)
	{
		// Figure out the direction the drone should be looking.
		Vector3 look = collider.transform.position - transform.position;

		// Project the look vector on the XZ plane.
		Vector3 droneLook = Vector3.ProjectOnPlane(look, Vector3.up);
		Quaternion rotation = Quaternion.LookRotation(droneLook);

		DroneTransform.rotation = SMath.RecursiveLerp(DroneTransform.rotation, rotation, 0.05f, Time.deltaTime);

		if (EyeTransform)
		{
			droneLook = Vector3.ProjectOnPlane(look, DroneTransform.right);
			rotation = Quaternion.LookRotation(droneLook);

			EyeTransform.rotation = SMath.RecursiveLerp(EyeTransform.rotation, rotation, 0.05f, Time.deltaTime);
		}

		// Attempt an attack.
		if (Time.time >= FireTime)
		{
			// Burst if necessary.
			if (BurstOn < BurstCount - 1)
			{
				FireTime = Time.time + BurstDelay;
				BurstOn++;

				Attack(context);
			}
			// Finish the attack.
			else
			{
				FireTime = Time.time + FireDelay;
				BurstOn = 0;

				Attack(context);
			}
		}

		Anger += 2 * Time.deltaTime;
	}

	private void Attack(EnemyContext context)
	{
		foreach (Transform Pupil in Pupils)
		{
			// Spawn the projectile and calculate a desired velocity for it.
			Vector3 location = Pupil.transform.position + Pupil.transform.forward * 0.2f;
			GameObject projectile = Instantiate(ProjectilePrefab, location, Pupil.transform.rotation);
			Vector3 velocity = Pupil.transform.forward * ProjectileSpeed + context.Velocity;

			// Actually apply the velocity and damage to the projectile.
			Projectile script = projectile.GetComponent<Projectile>();
			script.OnSpawn(velocity, Damage, "Enemy");
		}
	}
}
