using System;
using UnityEngine;

/// <summary>
/// Behavior Delegates are scripts that can communicate with each other.
/// </summary>
/// <typeparam name="TContext">
/// Behavior Context type used by the Behavior Delegate to pass object information.
/// </typeparam>
public abstract class BehaviorDelegate<TContext> : MonoBehaviour, IComparable<BehaviorDelegate<TContext>>
	where TContext : BehaviorContext, new()
{
	/// <summary>
	/// Controls which Behavior Delegates receive updates first.
	/// </summary>
	public int DelegateOrder { get; protected set; }

	/// <summary>
	/// Invoked once per Unity Update, but with a shared Behavior Context.
	/// </summary>
	/// <param name="context">Behavior Context for sharing data.</param>
	public abstract void UpdateDelegate(TContext context);

	/// <summary>
	/// Returns the difference in order values between the two Behavior Delegates.
	/// </summary>
	public int CompareTo(BehaviorDelegate<TContext> other)
	{
		return DelegateOrder - other.DelegateOrder;
	}
}
