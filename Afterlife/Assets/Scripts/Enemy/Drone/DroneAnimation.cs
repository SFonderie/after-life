using UnityEngine;

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

	void Start()
	{
		if (!InnerRoot)
		{
			// Too important; we're just using ourselves.
			InnerRoot = transform;
		}

		InnerCubes = InnerRoot.GetComponentsInChildren<BoxCollider>();
	}

	public override void UpdateDelegate(EnemyContext context)
	{
		InnerRadius = SMath.RecursiveLerp(InnerRadius, InnerRange, 0.1f, Time.deltaTime);

		float xFactor = InnerBias;
		float yFactor = 1 / xFactor;

		if (InnerCubes != null)
		{
			float slice = (2 * Mathf.PI) / (InnerCubes.Length);

			for (int i = 0; i < InnerCubes.Length; i++)
			{
				Transform cube = InnerCubes[i].transform;

				float angle = slice * i + InnerRotation;
				float radius = InnerRadius;

				float z = Mathf.Cos(angle) * radius * xFactor;
				float y = Mathf.Sin(angle) * radius * yFactor;

				cube.localPosition = new Vector3(0, y, z);
			}
		}

		InnerRotation += (Mathf.PI * InnerSpeed / 180) * Time.deltaTime;

		if (InnerRotation > Mathf.PI * 2)
		{
			InnerRotation -= Mathf.PI * 2;
		}
	}
}
