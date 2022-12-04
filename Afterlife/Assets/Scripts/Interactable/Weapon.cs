using UnityEngine;

public class Weapon : MonoBehaviour, IPlayerListener
{
	/// <summary>
	/// Scene listeners triggered by picking up the weapon.
	/// </summary>
	[SerializeField, Tooltip("Scene listeners triggered by picking up the weapon.")]
	private MonoBehaviour[] Listeners = null;

	public void OnHover(PlayerContext context)
	{

	}

	public void OnStartInteract(PlayerContext context)
	{
		// Play the weapon animation.
		context.DoWeaponPickup = true;

		// Hide and disable ourselves.
		gameObject.SetActive(false);

		// Trigger associated events.
		ISceneListener.DispatchEvents(Listeners);
	}

	public void OnStopInteract(PlayerContext context)
	{

	}

	public string GetActionName()
	{
		return "Take";
	}
}
