using UnityEngine;

/// <summary>
/// Behavior Contexts are container objects used to share game data.
/// </summary>
public abstract class BehaviorContext
{
	/// <summary>
	/// Maximum damage the entity can take before dying.
	/// </summary>
	[Header("General"), Min(1f), Tooltip("Maximum damage the entity can take before dying.")]
	public float Health = 100f;

	/// <summary>
	/// Total damage the entity has suffered.
	/// </summary>
	public float Damage { get; set; }

	/// <summary>
	/// Invoked once per Unity Awake.
	/// </summary>
	public virtual void Awake() { }

	/// <summary>
	/// Invoked once per Unity Start.
	/// </summary>
	public virtual void Start() { }

	/// <summary>
	/// Invoked once per Unity Update.
	/// </summary>
	public virtual void Update() { }

	/// <summary>
	/// Converts the context into a bool by performing a null check.
	/// </summary>
	public static implicit operator bool(BehaviorContext context) => context != null;
}