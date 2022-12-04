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

	/// <summary>
	/// List of listener scripts triggered by the fusebox.
	/// </summary>
	[SerializeField, Tooltip("List of listener scripts triggered by the fusebox.")]
	private MonoBehaviour[] Listeners = null;

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

		if (status)
		{
			Debug.Log("Breakers SUCCESS");
			// Play a nice sound here?

			// Dispatch level sequence events.
			ISceneListener.DispatchEvents(Listeners);
		}
		else
		{
			Debug.Log("Breakers FAIL");
			// Play a nice sound here?
		}
	}
}
