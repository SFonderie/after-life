using UnityEngine;

public class Narration : MonoBehaviour, IPlayerListener
{
	/// <summary>
	/// Actual text of the narration.
	/// </summary>
	[SerializeField, Tooltip("Actual text of the narration.")]
	private string NarrationText = "";

	/// <summary>
	/// Verb used when hovering over this trigger.
	/// </summary>
	[SerializeField, Tooltip("Verb used when hovering over this trigger.")]
	private string NarrationVerb = "Read";

	/// <summary>
	/// Duration of the text popup.
	/// </summary>
	[Min(1), SerializeField, Tooltip("Duration of the text popup.")]
	private float Duration = 5;

	/// <summary>
	/// Is the player reading this?
	/// </summary>
	[SerializeField, Tooltip("Is the player reading this?")]
	private bool Dialogue = true;

	public void OnHover(PlayerContext context)
	{

	}

	public void OnStartInteract(PlayerContext context)
	{
		if (Dialogue)
		{
			context.Dialogue = NarrationText;
			context.DialogueTime = Time.time + Duration;
		}
		else
		{
			context.Monologue = NarrationText;
			context.MonologueTime = Time.time + Duration;
		}
	}

	public void OnStopInteract(PlayerContext context)
	{

	}

	public string GetActionName()
	{
		return NarrationVerb;
	}
}
