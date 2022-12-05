using System;
using UnityEngine;

/// <summary>
/// Contains shared Enemy Object data.
/// </summary>
[Serializable]
public class EnemyContext : BehaviorContext
{
	/// <summary>
	/// How angry the enemy is.
	/// </summary>
	public float Anger { get; set; }

	/// <summary>
	/// Per-frame enemy velocity.
	/// </summary>
	public Vector3 Velocity { get; set; }
}
