using System;
using UnityEngine;

/// <summary>
/// Implement to allow a script to work with Scene Triggers.
/// </summary>
public interface ISceneListener
{
	/// <summary>
	/// Invoked by Scene Triggers once each time a player enters them.
	/// </summary>
	/// <param name="collision">Information about the player collision.</param>
	public void OnSceneTrigger(Collision collision);
}
