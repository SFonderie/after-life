using UnityEngine;

/// <summary>
/// Slides a door open or closed via a lerp between transforms.
/// </summary>
public class SlideDoor : MonoBehaviour, ISceneListener
{
	/// <summary>
	/// Door slide curve.
	/// </summary>
	[SerializeField, Tooltip("Door slide curve.")]
	private AnimationCurve SlideCurve = null;

	/// <summary>
	/// Position of the door when it is open.
	/// </summary>
	[SerializeField, Tooltip("Position of the door when it is open.")]
	private Transform OpenLocation = null;

	/// <summary>
	/// Position of the door when it is closed.
	/// </summary>
	[SerializeField, Tooltip("Position of the door when it is closed.")]
	private Transform CloseLocation = null;

	/// <summary>
	/// Time it takes for the door to open or close.
	/// </summary>
	[Range(0.05f, 1f), SerializeField, Tooltip("Time it takes for the door to open or close.")]
	private float DoorTime = 0.25f;

	/// <summary>
	/// If the door should be open by default.
	/// </summary>
	[SerializeField, Tooltip("If the door should be open by default.")]
	private bool OpenDefault = false;

	/// <summary>
	/// Current interpolation factor.
	/// </summary>
	float Interpolation = 0;

	/// <summary>
	/// Flag indicating the door state.
	/// </summary>
	bool Status = false;

	void Start()
	{
		if (!OpenLocation || !CloseLocation)
		{
			Debug.LogError("[SLIDING DOOR] - TRANSFORMS MISSING; DOOR DISABLED.");
		}

		if (OpenLocation == transform || CloseLocation == transform)
		{
			Debug.LogError("[SLIDING DOOR] - TRANSFORMS REFLEXIVE; DOOR DISABLED.");
		}
	}

	void Update()
	{
		if (OpenLocation && CloseLocation)
		{
			float increment = Time.deltaTime / DoorTime;
			Interpolation += Status ? increment : -increment;
			Interpolation = Mathf.Clamp(Interpolation, 0, 1);

			Transform A = OpenDefault ? OpenLocation : CloseLocation;
			Transform B = OpenDefault ? CloseLocation : OpenLocation;

			transform.position = Vector3.Lerp(A.position, B.position, SlideCurve.Evaluate(Interpolation));
			transform.rotation = Quaternion.Lerp(A.rotation, B.rotation, SlideCurve.Evaluate(Interpolation));
		}
	}

	public void OnPlayerEnter(Collider player)
	{
		Status = true;
	}

	public void OnPlayerExit(Collider player)
	{
		Status = false;
	}

	public void OnLevelSequence()
	{

	}
}
