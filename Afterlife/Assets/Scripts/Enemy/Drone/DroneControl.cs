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
	/// Size of the drone's awareness sphere.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Size of the drone's awareness sphere.")]
	private float VisionRadius = 10f;

	/// <summary>
	/// If true, forces the drone to ignore foes.
	/// </summary>
	[SerializeField, Tooltip("If true, forces the drone to ignore foes.")]
	private bool IgnoreEnemy = false;

	void Start()
	{
		if (!DroneTransform)
		{
			DroneTransform = transform;
		}
	}

	public override void UpdateDelegate(EnemyContext context)
	{
		if (!IgnoreEnemy)
		{
			foreach (Collider collider in Physics.OverlapSphere(transform.position, VisionRadius))
			{
				if (collider.gameObject.tag.Equals("Player"))
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
				}
			}
		}
	}
}
