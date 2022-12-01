using UnityEngine;

public class FuseKnob : MonoBehaviour, IPlayerListener
{
	/// <summary>
	/// Matching fusebox diode.
	/// </summary>
	[SerializeField, Tooltip("Matching fusebox diode.")]
	public MeshRenderer Diode = null;

	/// <summary>
	/// Knobs triggered by this knob.
	/// </summary>
	[SerializeField, Tooltip("Knobs triggered by this knob.")]
	private FuseKnob[] Cascade = null;

	/// <summary>
	/// Fusebox parent object. Needed for the Submit knob.
	/// </summary>
	[SerializeField, Tooltip("Fusebox parent object. Needed for the Submit knob.")]
	private FuseBox Parent = null;

	/// <summary>
	/// Hover label for this knob.
	/// </summary>
	[SerializeField, Tooltip("Hover label for this knob.")]
	private string Label = null;

	/// <summary>
	/// Starting state of the knob.
	/// </summary>
	[Tooltip("Starting state of the knob.")]
	public bool State = false;

	/// <summary>
	/// Is this the submit knob?
	/// </summary>
	[Tooltip("Is this a submit knob?")]
	public bool Submit = false;

	/// <summary>
	/// Is this the reset knob?
	/// </summary>
	[Tooltip("Is this the reset knob?")]
	public bool Reset = false;

	/// <summary>
	/// Knob turn animation curve.
	/// </summary>
	private AnimationCurve TurnCurve = null;

	/// <summary>
	/// Speed of rotation in angles per second.
	/// </summary>
	private float TurnTime = 0.15f;

	/// <summary>
	/// Current animation interpolation.
	/// </summary>
	private float Interpolate = 0;

	/// <summary>
	/// Rotation on angle.
	/// </summary>
	private const float AngleOn = -70;

	/// <summary>
	/// Rotation off angle.
	/// </summary>
	private const float AngleOff = 15;

	void Start()
	{
		Interpolate = State ? 0 : 1;
		TurnCurve = AnimationCurve.EaseInOut(0, AngleOff, 1, AngleOn);
	}

	public void OnHover(PlayerContext context)
	{

	}

	public void OnStartInteract(PlayerContext context)
	{
		SwitchKnob(!State);

		foreach (FuseKnob knob in Cascade)
		{
			if (Reset)
			{
				knob.SwitchKnob(false);
			}
			else
			{
				knob.SwitchKnob(!knob.State);
			}
		}
	}

	public void OnStopInteract(PlayerContext context)
	{

	}

	void Update()
	{
		float increment = Time.deltaTime / TurnTime;
		Interpolate += State ? increment : -increment;
		Interpolate = Mathf.Clamp(Interpolate, 0, 1);

		if (Interpolate == 1 && (Reset || Submit))
		{
			State = false;
		}

		Quaternion rotation = Quaternion.Euler(TurnCurve.Evaluate(Interpolate), 0, 0);
		transform.localRotation = rotation;

		if (Diode)
		{
			Diode.enabled = State;
		}
	}

	public void SwitchKnob(bool state)
	{
		State = state;

		if (Parent && Submit)
		{
			Parent.CheckBox();
		}
	}

	public string GetActionName()
	{
		return Label;
	}
}
