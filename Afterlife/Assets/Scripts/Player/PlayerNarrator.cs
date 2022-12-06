using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNarrator : PlayerDelegate
{
	/// <summary>
	/// Monologue text box.
	/// </summary>
	[SerializeField, Tooltip("Monologue text box.")]
	private TMP_Text Monologue = null;

	/// <summary>
	/// Dialogue text box.
	/// </summary>
	[SerializeField, Tooltip("Dialogue text box.")]
	private TMP_Text Dialogue = null;

	public override void HandleInput(InputAction.CallbackContext context)
	{

	}

	public override void UpdateDelegate(PlayerContext context)
	{
		Dialogue.text = Time.time >= context.DialogueTime ? "" : context.Dialogue;
		Monologue.text = Time.time >= context.MonologueTime ? "" : context.Monologue;

		Dialogue.enabled = Dialogue.text.Length > 0;
		Monologue.enabled = Monologue.text.Length > 0 && !(Dialogue.text.Length > 0);
	}
}
