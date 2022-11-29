using UnityEngine;

/// <summary>
/// Dispatches events to a list of listeners whenever a player enters a managed trigger.
/// </summary>
public class SceneTrigger : MonoBehaviour
{
	/// <summary>
	/// List of scripts listening for the Scene Trigger.
	/// </summary>
	[SerializeField, Tooltip("List of scripts listening for the Scene Trigger.")]
	private MonoBehaviour[] SceneListeners = null;

	void OnCollisionEnter(Collision collision)
	{
		// If the collision is with a player...
		if (collision.gameObject.tag.Equals("Player"))
		{
			foreach (MonoBehaviour candidate in SceneListeners)
			{
				if (candidate is ISceneListener listener)
				{
					listener.OnSceneTrigger(collision);
				}
			}
		}
	}
}
