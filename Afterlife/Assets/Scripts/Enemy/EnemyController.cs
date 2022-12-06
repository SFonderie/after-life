using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, ISceneListener
{
	[Serializable]
	public class EnemySpawnInfo
	{
		[Tooltip("Enemy prefab object.")]
		public GameObject EnemyObject = null;

		[Tooltip("Where the enemy should move once spawned.")]
		public Transform SpawnTarget = null;

		[Min(0), Tooltip("How long the enemy should wait before moving.")]
		public float SpawnDelay = 0;

		[Min(0.1f), Tooltip("Time in seconds it takes for the enemy to move to the target.")]
		public float SpawnTime = 1;

		[NonSerialized]
		public DroneControl Control = null;

		[NonSerialized]
		public AnimationCurve Curve = null;

		[NonSerialized]
		public Vector3 Start = Vector3.zero;

		[NonSerialized]
		public float Time = 0;

		public bool Active => Curve != null;
	}

	[SerializeField, Tooltip("List of enemies managed by this spawner.")]
	private EnemySpawnInfo[] Enemies = null;

	[SerializeField, Tooltip("List of scene listeners that are trigger by this spawner.")]
	private MonoBehaviour[] Listeners = null;

	[SerializeField, Tooltip("Should this spawner trigger as a level sequence?")]
	private bool Sequence = false;

	private bool Active = false;

	private bool Alive = true;

	private bool Complete = false;

	void Start()
	{
		foreach (EnemySpawnInfo Enemy in Enemies)
		{
			if (Enemy.EnemyObject)
			{
				Enemy.Control = Enemy.EnemyObject.GetComponent<DroneControl>();
				Enemy.Control.IgnoreEnemy = true;

				if (Enemy.SpawnTime <= 0)
				{
					Enemy.SpawnTime = 1;
				}
			}
		}
	}

	void Update()
	{
		// Skip updates until triggered.
		// Skip updates if we're done.
		if (!Active || Complete)
		{
			return;
		}

		Alive = false;

		foreach (EnemySpawnInfo Enemy in Enemies)
		{
			// No transform provided; skip.
			if (!Enemy.SpawnTarget)
			{
				continue;
			}

			// Start the sequence!
			if (Enemy.Active)
			{
				Alive = Alive || Enemy.EnemyObject;

				// The enemy is dead now; skip.
				if (!Enemy.EnemyObject)
				{
					continue;
				}

				Enemy.Time += Time.deltaTime;

				// Figure out where in the spawn sequence this enemy object is...
				float interpolate = (Enemy.Time - Enemy.SpawnDelay) / Enemy.SpawnTime;

				// Only attack after the spawn delay is done.
				Enemy.Control.IgnoreEnemy = interpolate < 1;

				// Clamp the rest for animation purposes.
				interpolate = Mathf.Clamp(interpolate, 0, 1);

				// Smoothly interpolate between the saved start position and the target position using our scaled value.
				Vector3 position = Vector3.Lerp(Enemy.Start, Enemy.SpawnTarget.position, Enemy.Curve.Evaluate(interpolate));
				Enemy.EnemyObject.transform.position = position;
			}
			else
			{
				// No enemy provided; skip.
				if (!Enemy.EnemyObject || !Enemy.Control)
				{
					continue;
				}

				Enemy.Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
				Enemy.Start = Enemy.EnemyObject.transform.position;
				Enemy.Time = 0;
				Alive = true;
			}
		}

		if (!Alive)
		{
			ISceneListener.DispatchEvents(Listeners);
			Complete = true;
		}
	}

	public void OnPlayerEnter(Collider player)
	{
		Active = Active || !Sequence;
	}

	public void OnPlayerExit(Collider player)
	{

	}

	public void OnLevelSequence()
	{
		Active = Active || Sequence;
	}
}
