using UnityEngine;

public class Weapon : MonoBehaviour, IPlayerListener
{
	public void OnHover(PlayerContext context)
	{

	}

	public void OnStartInteract(PlayerContext context)
	{
		// Play the weapon animation.
		context.DoWeaponPickup = true;

		// Hide and disable ourselves.
		gameObject.SetActive(false);
	}

	public void OnStopInteract(PlayerContext context)
	{

	}

	public string GetActionName()
	{
		return "Take";
	}
}
