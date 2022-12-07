using UnityEngine;

public class Tape : MonoBehaviour, IPlayerListener
{
	/// <summary>
	/// Listeners triggered by this tape being picked up. Not that you'd actually need any though...
	/// </summary>
	[SerializeField, Tooltip("Listeners triggered by this tape being picked up. Not that you'd actually need any though...")]
	private MonoBehaviour[] Listeners = null;

	public void OnHover(PlayerContext context)
	{

	}

	public void OnStartInteract(PlayerContext context)
	{
		// Up the tape state by one.
		context.TapeState++;

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
