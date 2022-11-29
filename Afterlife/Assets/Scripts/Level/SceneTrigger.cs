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

	void OnTriggerEnter(Collider other)
	{
		OnTriggerSwitch(other, true);
	}

	void OnTriggerExit(Collider other)
	{
		OnTriggerSwitch(other, false);
	}

	private void OnTriggerSwitch(Collider other, bool enter)
	{
		// If the collision is with a player...
		if (other.gameObject.tag.Equals("Player"))
		{
			foreach (MonoBehaviour candidate in SceneListeners)
			{
				if (candidate is ISceneListener listener)
				{
					if (enter)
					{
						listener.OnPlayerEnter(other);
					}
					else
					{
						listener.OnPlayerExit(other);
					}
				}
			}
		}
	}
}
