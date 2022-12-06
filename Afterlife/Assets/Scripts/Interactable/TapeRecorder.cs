using UnityEngine;

public class TapeRecorder : MonoBehaviour, IPlayerListener
{
	/// <summary>
	/// Tapes needed to "complete" this recorder. Each will display as dialogue.
	/// </summary>
	[SerializeField, Tooltip("Tapes needed to \"complete\" this recorder. Each will display as dialogue.")]
	private string[] TapeContents = null;

	/// <summary>
	/// Time in seconds that each tape will play.
	/// </summary>
	[SerializeField, Tooltip("Time in seconds that each tape will play.")]
	private float TapeDuration = 10;

	/// <summary>
	/// Listeners triggered by the tape recorder playing all of its tapes.
	/// </summary>
	[SerializeField, Tooltip("Listeners triggered by the tape recorder playing all of its tapes.")]
	private MonoBehaviour[] Listeners = null;

	/// <summary>
	/// Does the player have a tape?
	/// </summary>
	private bool HasTape = false;

	/// <summary>
	/// Current tape.
	/// </summary>
	private int TapeOn = 0;

	public void OnHover(PlayerContext context)
	{
		HasTape = context.TapeState > 0;
	}

	public void OnStartInteract(PlayerContext context)
	{
		// The player has an unplayed tape.
		if (context.TapeState > 0 && TapeOn < TapeContents.Length)
		{
			context.TapeState--;
			TapeOn++;

			context.Dialogue = TapeContents[TapeOn - 1];
			context.DialogueTime = Time.time + TapeDuration;
		}

		// We are now done with the tapes...
		if (TapeOn == TapeContents.Length)
		{
			ISceneListener.DispatchEvents(Listeners);
		}
	}

	public void OnStopInteract(PlayerContext context)
	{

	}

	public string GetActionName()
	{
		return HasTape ? "Play Tape " + (TapeOn + 1) : "No Tape";
	}
}
