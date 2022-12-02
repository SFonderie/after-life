using UnityEngine;

/// <summary>
/// Implement to allow a script to work with Scene Triggers.
/// </summary>
public interface ISceneListener
{
	/// <summary>
	/// Invoked by Scene Triggers once each time a player enters them.
	/// </summary>
	/// <param name="player">Player collider component.</param>
	public void OnPlayerEnter(Collider player);

	/// <summary>
	/// Invoked by Scene Triggers once each time a player exits them.
	/// </summary>
	/// <param name="player">Player collider component.</param>
	public void OnPlayerExit(Collider player);

	/// <summary>
	/// Invoked by certain assets whenever a player triggers them.
	/// </summary>
	public void OnLevelSequence();
}
