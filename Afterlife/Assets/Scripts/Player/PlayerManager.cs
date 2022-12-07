using UnityEngine;
using UnityEngine.InputSystem;
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
}
