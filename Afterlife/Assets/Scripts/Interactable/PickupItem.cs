using UnityEngine;
using UnityEngine.LowLevel;

/// <summary>
/// Item that can be picked up and inspected by the player.
/// </summary>
public class PickupItem : MonoBehaviour, IPlayerListener
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

	/// <summary>
	/// Is the player inspecting right now?
	/// </summary>
	private bool Inspecting = false;

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
	}

	public void OnHover(PlayerContext context)
	{

	}

	public void OnInteraction(PlayerContext context)
	{
		Inspecting = !context.Inspecting;
		context.Inspecting = Inspecting;

		Target = Inspecting ? context.InspectTransform : DefaultTransform;
	}

	void Update()
	{
		if (Target)
		{
			ItemTransform.position = SMath.RecursiveLerp(ItemTransform.position, Target.position, 0.1f, 4 * Time.deltaTime);
			ItemTransform.rotation = SMath.RecursiveLerp(ItemTransform.rotation, Target.rotation, 0.1f, 8 * Time.deltaTime);
		}
	}
}
