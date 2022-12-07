using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior Manager is the script that allows Behavior Delegates to communicate.
/// </summary>
/// <typeparam name="TDelegate">Behavior Delegate type managed by the Behavior Manager.</typeparam>
/// <typeparam name="TContext">Behavior Context type used by the Behavior Delegate type.</typeparam>
public abstract class BehaviorManager<TDelegate, TContext> : MonoBehaviour
	where TDelegate : BehaviorDelegate<TContext>
	where TContext : BehaviorContext, new()
{
	/// <summary>
	/// Editor-visible Behavior Context. Allows designers to modify base values in the editor.
	/// </summary>
	[SerializeField, Tooltip("Behavior Context passed to managed Behavior Delegates.")]
	private TContext _context = null;

	/// <summary>
	/// Behavior Context passed to managed Behavior Delegates.
	/// </summary>
	public TContext Context => _context;

	/// <summary>
	/// List of all managed Behavior Delegates sorted by their order values.
	/// </summary>
	protected List<TDelegate> Delegates { get; } = new List<TDelegate>();

	public virtual void Awake()
	{
		LocateDelegates();
		Context.Awake();
	}

	public virtual void Start()
	{
		Context.Start();
	}

	public virtual void Update()
	{
		Context.Update();

		foreach (TDelegate script in Delegates)
		{
			if (script.enabled)
			{
				script.UpdateDelegate(Context);
			}
		}
	}

	/// <summary>
	/// Searches the Game Object and its children for managable Behavior Delegate scripts.
	/// </summary>
	public void LocateDelegates()
	{
		if (Delegates.Count > 0)
		{
			Delegates.Clear();
		}

		Delegates.AddRange(GetComponents<TDelegate>());
		Delegates.AddRange(GetComponentsInChildren<TDelegate>());
		Delegates.Sort();
	}

	public void TakeDamage(float damage)
	{
		Context.Damage += damage;

		if (Context.Damage >= Context.Health)
		{
			OnDeath();
		}

		Debug.Log("Health is " + (Context.Health - Context.Damage) + " / " + Context.Health);
		OnDamage();
	}

	public virtual void OnDamage()
	{
		// Override to create damage effects.
	}

	public virtual void OnDeath()
	{
		// Override to create death effects.
	}
}
