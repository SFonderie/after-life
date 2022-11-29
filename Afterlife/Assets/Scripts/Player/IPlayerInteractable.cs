using UnityEngine;

public interface IPlayerListener
{
	/// <summary>
	/// Invoked each time that the player interacts with this object.
	/// </summary>
	public void OnInteraction(GameObject player);
}