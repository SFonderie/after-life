using UnityEngine;
using TMPro;

/// <summary>
/// Creates a keypad asset.
/// </summary>
public class Keypad : MonoBehaviour, IPlayerListener
{
	/// <summary>
	/// UI Panel containing the keypad interface.
	/// </summary>
	[SerializeField, Tooltip("UI Panel containing the keypad interface.")]
	private GameObject KeypadPanel = null;

	/// <summary>
	/// Text UI asset for displaying the current code.
	/// </summary>
	[SerializeField, Tooltip("Text UI asset for displaying the current code.")]
	private TMP_Text CodeText = null;

	/// <summary>
	/// The actual keypad code.
	/// </summary>
	[SerializeField, Tooltip("The actual keypad code.")]
	private int CorrectCode = 0;

	/// <summary>
	/// List of listener scripts triggered by the keypad.
	/// </summary>
	[SerializeField, Tooltip("List of scene listeners triggered by the keypad.")]
	private MonoBehaviour[] Listeners = null;

	/// <summary>
	/// Local reference to the player context; used to force-interact.
	/// </summary>
	private PlayerContext local = null;

	/// <summary>
	/// Current entered code value.
	/// </summary>
	private string code = "";

	/// <summary>
	/// Current length of the entered code digits.
	/// </summary>
	private int currentCodeLength = 0;

	/// <summary>
	/// Maximum length of the entered code.
	/// </summary>
	private const int maxCodeLength = 4;

	void Start()
	{
		KeypadPanel.SetActive(false);
		CodeText.text = "_ _ _ _";
	}

	public void OnHover(PlayerContext context)
	{

	}

	public void OnStartInteract(PlayerContext context)
	{
		// Steal the context.
		local = context;

		// Lock the player in place.
		context.Interacting = true;
		context.Inspecting = true;

		// Reveal the cursor.
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;

		// Reveal the keypad UI.
		KeypadPanel.SetActive(true);
	}

	public void OnStopInteract(PlayerContext context)
	{
		// Lock the cursor.
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		// Hide the panel.
		KeypadPanel.SetActive(false);
	}

	void Update()
	{

	}

	public string GetActionName()
	{
		return "Use";
	}

	public void AddCodeDigit(int digit)
	{
		currentCodeLength++;

		if (currentCodeLength > maxCodeLength)
		{
			currentCodeLength = 1;
			code = digit.ToString();
			CodeText.text = code + " _ _ _";
		}
		else
		{
			code = code + " " + digit.ToString();
			string temp_code = code;

			for (int i = 0; i < maxCodeLength - currentCodeLength; i++)
			{
				temp_code += " _";
			}

			CodeText.text = temp_code;
		}

		Debug.Log(code);
	}

	public void CheckCode()
	{
		if (code.Replace(" ", "").Equals(CorrectCode.ToString()))
		{
			Debug.Log("Code CONFIRMED");
			// Play a nice sound?

			// Force out the keypad.
			OnStopInteract(local);
			local.Interacting = false;
			local.Inspecting = false;
			local = null;

			foreach (MonoBehaviour script in Listeners)
			{
				if (script is ISceneListener listener)
				{
					listener.OnLevelSequence();
				}
			}
		}
		else
		{
			Debug.Log("Code REJECTED");
			// Play a not nice sound.

			code = "";
			CodeText.text = "_ _ _ _";
			currentCodeLength = 0;
		}
	}

}
