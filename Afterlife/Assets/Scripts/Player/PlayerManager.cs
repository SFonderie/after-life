using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages Player Behavior scripts.
/// </summary>
public class PlayerManager : BehaviorManager<PlayerDelegate, PlayerContext>
{
	/// <summary>
	/// Player Input component reference. Used to source input events.
	/// </summary>
	[SerializeField, Tooltip("Player Input component reference. Used to source input events.")]
	private PlayerInput _input = null;

	/// <summary>
	/// Player injury overlay image.
	/// </summary>
	[SerializeField, Tooltip("Player injury overlay image.")]
	private Image InjuryImage = null;

	/// <summary>
	/// Paused game panel.
	/// </summary>
	[SerializeField, Tooltip("Death panel.")]
	private GameObject DeathPanel = null;

	/// <summary>
	/// Main menu loading slider.
	/// </summary>
	[SerializeField, Tooltip("Death loading slider.")]
	private Slider DeathLoad = null;

	private const float MaxAlpha = 40f / 255f;
	private float ActualAlpha = 0;
	private float TargetAlpha = 0;
	private float CurrentAlpha = 0;

	public override void Start()
	{
		base.Start();

		if (_input)
		{
			_input.onActionTriggered += HandleInput;
		}

		DeathPanel.SetActive(false);
		DeathLoad.gameObject.SetActive(false);
	}

	public override void Update()
	{
		base.Update();

		ActualAlpha = Mathf.Lerp(0, MaxAlpha, Context.Damage / Context.Health);
		TargetAlpha = SMath.RecursiveLerp(TargetAlpha, ActualAlpha, 0.05f, Time.deltaTime);
		CurrentAlpha = SMath.RecursiveLerp(CurrentAlpha, TargetAlpha, 0.001f, Time.deltaTime);

		if (InjuryImage)
		{
			Color color = InjuryImage.color;
			color.a = CurrentAlpha;
			InjuryImage.color = color;
		}
	}

	/// <summary>
	/// Passes input events to all Player Delegates.
	/// </summary>
	/// <param name="context">Input event context struct.</param>
	public void HandleInput(InputAction.CallbackContext context)
	{
		foreach (PlayerDelegate script in Delegates)
		{
			script.HandleInput(context);
		}
	}

	public override void OnDamage()
	{
		TargetAlpha = MaxAlpha * 3;
	}

	public override void OnDeath()
	{
		DeathPanel.SetActive(true);

		Context.Dead = true;

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	public void OnReload()
	{
		DeathLoad.gameObject.SetActive(true);
		Scene scene = SceneManager.GetActiveScene();
		StartCoroutine(LoadScene(scene.name));
	}

	public void OnMainMenu()
	{
		DeathLoad.gameObject.SetActive(true);
		StartCoroutine(LoadScene("MainMenu"));
	}

	IEnumerator LoadScene(string name)
	{
		AsyncOperation loader = SceneManager.LoadSceneAsync(name);

		while (!loader.isDone)
		{
			if (DeathLoad)
			{
				DeathLoad.value = loader.progress;
			}

			yield return null;
		}
	}
}
