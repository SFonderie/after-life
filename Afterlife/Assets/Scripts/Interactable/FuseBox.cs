using UnityEngine;

public class FuseBox : MonoBehaviour
{
	/// <summary>
	/// Solution set.
	/// </summary>
	[SerializeField, Tooltip("Solution set.")]
	private bool[] Solution = null;

	/// <summary>
	/// Array of all fusebox knobs.
	/// </summary>
	[SerializeField, Tooltip("Array of all fusebox knobs.")]
	private FuseKnob[] Knobs = null;

	[SerializeField, Tooltip("Is this fusebox a one-shot?")]
	private bool OneShot = true;

	/// <summary>
	/// List of listener scripts triggered by the fusebox.
	/// </summary>
	[SerializeField, Tooltip("List of listener scripts triggered by the fusebox.")]
	private MonoBehaviour[] Listeners = null;

	/// <summary>
	/// Has the fuse box fired yet?
	/// </summary>
	private bool Fired = false;

	void Start()
	{
		if (Solution.Length != Knobs.Length)
		{
			Debug.Log("[FUSEBOX] - SOLUTION / KNOB MISMATCH; USING SHORTER SET.");
		}
	}

	public void CheckBox()
	{
		int length = Knobs.Length > Solution.Length ? Solution.Length : Knobs.Length;
		bool status = true;

		for (int i = 0; i < length; i++)
		{
			status = status && Knobs[i].State == Solution[i];
		}

		if (status && (!Fired || !OneShot))
		{
			Debug.Log("Breakers SUCCESS");
			// Play a nice sound here?

			// Dispatch level sequence events.
			ISceneListener.DispatchEvents(Listeners);

			Fired = true;
		}
		else
		{
			Debug.Log("Breakers FAIL");
			// Play a nice sound here?
		}
	}
}
