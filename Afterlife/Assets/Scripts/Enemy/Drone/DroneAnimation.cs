using UnityEngine;

/// <summary>
/// Handles animating the drone enemy.
/// </summary>
public class DroneAnimation : EnemyDelegate
{
	/// <summary>
	/// Transform root of the inner cubes.
	/// </summary>
	[SerializeField, Tooltip("Transform root of the inner cubes.")]
	private Transform InnerRoot = null;

	/// <summary>
	/// Base speed of rotation for the inner ring in degrees per second.
	/// </summary>
	[SerializeField, Tooltip("Base speed of rotation for the inner ring in degrees per second.")]
	private float InnerSpeed = 30;

	/// <summary>
	/// Base radius for the inner cube rotation.
	/// </summary>
	[SerializeField, Range(0f, 2f), Tooltip("Base radius for the inner cube rotation.")]
	private float InnerRange = 1;

	/// <summary>
	/// Inverse factor controlling how heavily the cubes favor movement in the x-axis.
	/// </summary>
	[SerializeField, Range(0f, 2f), Tooltip("Inverse factor controlling how heavily the cubes favor movement in the x-axis.")]
	private float InnerBias = 1;

	/// <summary>
	/// Transform root of the outer cubes.
	/// </summary>
	[SerializeField, Tooltip("Transform root of the outer cubes.")]
	private Transform OuterRoot = null;

	/// <summary>
	/// Base speed of rotation for the outer ring in degrees per second.
	/// </summary>
	[SerializeField, Tooltip("Base speed of rotation for the outer ring in degrees per second.")]
	private float OuterSpeed = -30;

	/// <summary>
	/// Base radius for the outer cube rotation.
	/// </summary>
	[SerializeField, Range(0f, 4f), Tooltip("Base radius for the outer cube rotation.")]
	private float OuterRange = 2;

	/// <summary>
	/// Inverse factor controlling how heavily the cubes favor movement in the x-axis.
	/// </summary>
	[SerializeField, Range(0f, 2f), Tooltip("Inverse factor controlling how heavily the cubes favor movement in the x-axis.")]
	private float OuterBias = 1;

	/// <summary>
	/// Inner cubes box collider reference.
	/// </summary>
	private BoxCollider[] InnerCubes = null;

	/// <summary>
	/// Current global rotation for the inner cubes.
	/// </summary>
	private float InnerRotation = 0;

	/// <summary>
	/// Current global radius for the inner cubes.
	/// </summary>
	private float InnerRadius = 0;

	/// <summary>
	/// Current global speed for the inner cubes.
	/// </summary>
	private float InnerRate = 0;

	/// <summary>
	/// Outer cubes box collider reference.
	/// </summary>
	private BoxCollider[] OuterCubes = null;

	/// <summary>
	/// Current global rotation for the outer cubes.
	/// </summary>
	private float OuterRotation = 0;

	/// <summary>
	/// Current global radius for the outer cubes.
	/// </summary>
	private float OuterRadius = 0;

	/// <summary>
	/// Current global speed for the outer cubes.
	/// </summary>
	private float OuterRate = 0;

	void Start()
	{
		if (!InnerRoot)
		{
			// Too important; we're just using ourselves.
			InnerRoot = transform;
		}

		InnerCubes = InnerRoot.GetComponentsInChildren<BoxCollider>();

		if (OuterRoot)
		{
			OuterCubes = OuterRoot.GetComponentsInChildren<BoxCollider>();
		}
	}

	public override void UpdateDelegate(EnemyContext context)
	{
		InnerRadius = SMath.RecursiveLerp(InnerRadius, Mathf.Lerp(0, InnerRange, context.Anger), 0.1f, Time.deltaTime);
		OuterRadius = SMath.RecursiveLerp(OuterRadius, Mathf.Lerp(0, OuterRange, context.Anger), 0.1f, Time.deltaTime);

		InnerRate = SMath.RecursiveLerp(InnerRate, Mathf.Lerp(0, InnerSpeed, context.Anger), 0.1f, Time.deltaTime);
		OuterRate = SMath.RecursiveLerp(OuterRate, Mathf.Lerp(0, OuterSpeed, context.Anger), 0.1f, Time.deltaTime);

		float ixFactor = InnerBias;
		float iyFactor = 1 / ixFactor;

		float oxFactor = OuterBias;
		float oyFactor = 1 / oxFactor;

		if (InnerCubes != null)
		{
			float slice = (2 * Mathf.PI) / (InnerCubes.Length);

			for (int i = 0; i < InnerCubes.Length; i++)
			{
				Transform cube = InnerCubes[i].transform;

				float angle = slice * i + InnerRotation;
				float radius = InnerRadius;

				float x = Mathf.Cos(angle) * radius * ixFactor;
				float y = Mathf.Sin(angle) * radius * iyFactor;

				cube.localPosition = new Vector3(x, y, 0);
			}
		}

		if (OuterCubes != null)
		{
			float slice = (2 * Mathf.PI) / (OuterCubes.Length);

			for (int i = 0; i < OuterCubes.Length; i++)
			{
				Transform cube = OuterCubes[i].transform;

				float angle = slice * i + OuterRotation;
				float radius = OuterRadius;

				float x = Mathf.Cos(angle) * radius * oxFactor;
				float y = Mathf.Sin(angle) * radius * oyFactor;

				cube.localPosition = new Vector3(x, y, 0);
			}
		}

		InnerRotation += (Mathf.PI * InnerRate / 180) * Time.deltaTime;
		OuterRotation += (Mathf.PI * OuterRate / 180) * Time.deltaTime;

		if (InnerRotation > Mathf.PI * 2)
		{
			InnerRotation -= Mathf.PI * 2;
		}

		if (InnerRotation < -Mathf.PI * 2)
		{
			InnerRotation += Mathf.PI * 2;
		}

		if (OuterRotation > Mathf.PI * 2)
		{
			OuterRotation -= Mathf.PI * 2;
		}

		if (OuterRotation < Mathf.PI * 2)
		{
			OuterRotation += Mathf.PI * 2;
		}
	}
}
