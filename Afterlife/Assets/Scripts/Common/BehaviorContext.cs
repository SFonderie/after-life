/// <summary>
/// Behavior Contexts are container objects used to share game data.
/// </summary>
public abstract class BehaviorContext
{
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