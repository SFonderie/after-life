using UnityEngine;

/// <summary>
/// Item that can be picked up and inspected by the player.
/// </summary>
public class Pickup : MonoBehaviour, IPlayerListener
{
	/// <summary>
	/// Where the item rests when not being held.
	/// </summary>
	[SerializeField, Tooltip("Where the item rests when not being held.")]
	private Transform DefaultTransform = null;

	/// <summary>
	/// Actual item transform.
	/// </summary>
	[SerializeField, Tooltip("Actual item transform.")]
	private Transform ItemTransform = null;

	/// <summary>
	/// Current target transform.
	/// </summary>
	private Transform Target = null;

	void Start()
	{
		if (!ItemTransform)
		{
			Debug.LogError("[PICKUP ITEM] - NO ITEM TRANSFORM LINKED; INTERACTION DISABLED.");
		}

		if (!DefaultTransform)
		{
			DefaultTransform = transform;
		}

		Target = DefaultTransform;
	}

	public void OnHover(PlayerContext context)
	{

	}

	public void OnStartInteract(PlayerContext context)
	{
		Target = context.PickupTransform;
		context.Interacting = true;
	}

	public void OnStopInteract(PlayerContext context)
	{
		Target = DefaultTransform;
	}

	void Update()
	{
		if (Target)
		{
			ItemTransform.position = SMath.RecursiveLerp(ItemTransform.position, Target.position, 0.1f, 4 * Time.deltaTime);
			ItemTransform.rotation = SMath.RecursiveLerp(ItemTransform.rotation, Target.rotation, 0.1f, 8 * Time.deltaTime);

			// Only switch back to the default layer once the object is safely in place.
			if ((DefaultTransform.position - ItemTransform.position).sqrMagnitude < 0.0001f)
			{
				ItemTransform.gameObject.layer = LayerMask.NameToLayer("Default");
			}
			else
			{
				ItemTransform.gameObject.layer = LayerMask.NameToLayer("Pickup");
			}
		}
	}

	public string GetActionName()
	{
		return "Read";
	}
}
