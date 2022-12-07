using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Implements player look-based interaction.
/// </summary>
public class PlayerInteraction : PlayerDelegate
{
	/// <summary>
	/// Player camera transform reference.
	/// </summary>
	[Header("Components"), SerializeField, Tooltip("Player camera transform reference.")]
	private Transform PlayerLook = null;

	/// <summary>
	/// Player inspection position.
	/// </summary>
	[SerializeField, Tooltip("Player inspection position.")]
	private Transform PlayerInspect = null;

	/// <summary>
	/// Interaction tooltip and crosshair panel.
	/// </summary>
	[SerializeField, Tooltip("Interaction tooltip and crosshair panel.")]
	private GameObject InteractPanel = null;

	/// <summary>
	/// Paused game panel.
	/// </summary>
	[SerializeField, Tooltip("Paused game panel.")]
	private GameObject PausePanel = null;

	/// <summary>
	/// Main menu loading slider.
	/// </summary>
	[SerializeField, Tooltip("Main menu loading slider.")]
	private Slider MenuLoad = null;

	/// <summary>
	/// Interaction tooltip text.
	/// </summary>
	[SerializeField, Tooltip("Interaction tooltip text.")]
	private TMP_Text InteractText = null;

	/// <summary>
	/// Interaction sphere cast length.
	/// </summary>
	[Header("Variables"), Min(0), SerializeField, Tooltip("Interaction sphere cast length.")]
	private float InteractRange = 3f;

	/// <summary>
	/// Interaction sphere cast radius.
	/// </summary>
	[Min(0), SerializeField, Tooltip("Interaction sphere cast radius.")]
	private float InteractRadius = 0.25f;

	/// <summary>
	/// Current tracked player listener.
	/// </summary>
	private IPlayerListener Listener = null;

	/// <summary>
	/// Should an interaction occur this frame?
	/// </summary>
	private bool DoInteract = false;

	/// <summary>
	/// Should interaction be ignored?
	/// </summary>
	private bool BlockInteract = false;

	/// <summary>
	/// Should a pause occur this frame?
	/// </summary>
	private bool DoPause = false;

	/// <summary>
	/// Should pauses be ignored?
	/// </summary>
	private bool BlockPause = false;

	/// <summary>
	/// Special bool; DO NOT TOUCH.
	/// </summary>
	private bool DoUnpause = false;

	void Start()
	{
		if (!PlayerLook)
		{
			Debug.LogError("[PLAYER INTERACTION] - NO CAMERA TRANSFORM LINKED; INTERACTION DISABLED.");
		}

		if (!PlayerInspect)
		{
			Debug.LogError("[PLAYER INTERACTION] - NO INSPECTION TRANSFORM LINKED; INTERACTION DISABLED.");
		}

		MenuLoad.gameObject.SetActive(false);
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{
		if (context.action.name.Equals("Interact"))
		{
			DoInteract = context.performed && !BlockInteract;
			BlockInteract = BlockInteract && !(BlockInteract && context.canceled);
		}

		if (context.action.name.Equals("Pause"))
		{
			DoPause = context.performed && !BlockPause;
			BlockPause = BlockPause && !(BlockPause && context.canceled);
		}
	}

	void FixedUpdate()
	{
		if (!PlayerLook || !PlayerInspect)
		{
			return;
		}

		foreach (RaycastHit hit in Physics.SphereCastAll(PlayerLook.position, InteractRadius, PlayerLook.forward, InteractRange))
		{
			IPlayerListener component = hit.transform.GetComponent<IPlayerListener>();

			if (component != null)
			{
				Listener = component;
				return;
			}
		}

		Listener = null;
	}

	public override void UpdateDelegate(PlayerContext context)
	{
		if (((DoPause && !BlockPause) || DoUnpause) && PausePanel)
		{
			context.Paused = !context.Paused;

			// WARNING! NUCLEAR CODE DETECTED.
			Time.timeScale = context.Paused ? 0 : 1;

			Cursor.lockState = context.Paused ? CursorLockMode.Confined : CursorLockMode.Locked;
			Cursor.visible = context.Paused;

			if (!DoUnpause)
			{
				DoPause = false;
				BlockPause = true;
			}
			else
			{
				DoUnpause = false;
				DoPause = false;
				BlockPause = false;
			}

			if (context.Interacting)
			{
				Listener.OnStopInteract(context);
				context.Interacting = false;
				context.Inspecting = false;
			}
		}

		if (!PlayerLook || !PlayerInspect)
		{
			return;
		}

		context.PickupTransform = PlayerInspect;

		if (Listener != null && !context.Paused)
		{
			Listener.OnHover(context);

			if (DoInteract && !BlockInteract)
			{
				if (context.Interacting)
				{
					Listener.OnStopInteract(context);
					context.Interacting = false;
					context.Inspecting = false;
				}
				else
				{
					Listener.OnStartInteract(context);
				}

				DoInteract = false;
				BlockInteract = true;
			}
		}

		if (InteractPanel)
		{
			InteractPanel.SetActive(!context.Interacting && !context.Paused);
		}

		if (PausePanel)
		{
			PausePanel.SetActive(context.Paused);
		}

		if (InteractText)
		{
			InteractText.text = Listener != null ? Listener.GetActionName() + " [E]" : "";
		}
	}

	/// <summary>
	/// Invoked by buttons to unpause the game.
	/// </summary>
	public void Unpause()
	{
		DoUnpause = true;
	}

	public void LoadMainMenu()
	{
		MenuLoad.gameObject.SetActive(true);
		StartCoroutine(LoadScene());
	}

	IEnumerator LoadScene()
	{
		AsyncOperation loader = SceneManager.LoadSceneAsync("MainMenu");

		while (!loader.isDone)
		{
			if (MenuLoad)
			{
				MenuLoad.value = loader.progress;
			}

			yield return null;
		}
	}
}
