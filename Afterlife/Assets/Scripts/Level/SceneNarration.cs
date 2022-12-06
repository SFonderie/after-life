using UnityEngine;

public class SceneNarration : MonoBehaviour, ISceneListener
{
	/// <summary>
	/// Actual text of the narration.
	/// </summary>
	[SerializeField, Tooltip("Actual text of the narration.")]
	private string NarrationText = "";

	/// <summary>
	/// Duration of the text popup.
	/// </summary>
	[Min(1), SerializeField, Tooltip("Duration of the text popup.")]
	private float Duration = 5;

	/// <summary>
	/// Is this the player thinking to themselves?
	/// </summary>
	[SerializeField, Tooltip("Is this the player thinking to themselves?")]
	private bool Monologue = true;

	/// <summary>
	/// Is this narration part of a level sequence?
	/// </summary>
	[SerializeField, Tooltip("Is this narration part of a level sequence?")]
	private bool Sequence = false;

	/// <summary>
	/// Does this narration only fire once?
	/// </summary>
	[SerializeField, Tooltip("Does this narration only fire once?")]
	private bool OneShot = true;

	/// <summary>
	/// Player manager reference. Used to access contexts.
	/// </summary>
	private PlayerManager Manager = null;

	/// <summary>
	/// Has the narration fired yet?
	/// </summary>
	private bool Fired = false;

	void Start()
	{
		GameObject Player = GameObject.FindGameObjectWithTag("Player");
		Manager = Player.GetComponent<PlayerManager>();
	}

	public void OnPlayerEnter(Collider player)
	{
		if (Manager && !Sequence) // Should we even try to fire the narration?
		{
			FireEvent();
		}
	}

	public void OnPlayerExit(Collider player)
	{

	}

	public void OnLevelSequence()
	{
		if (Manager && Sequence) // Should we even try to fire the narration?
		{
			FireEvent();
		}
	}

	private void FireEvent()
	{
		if (!Fired || !OneShot) // Only fire if we haven't before, or aren't a one-shot.
		{
			if (Monologue)
			{
				Manager.Context.Monologue = NarrationText;
				Manager.Context.MonologueTime = Time.time + Duration;
			}
			else
			{
				Manager.Context.Dialogue = NarrationText;
				Manager.Context.DialogueTime = Time.time + Duration;
			}

			Fired = true;
		}
	}
}
