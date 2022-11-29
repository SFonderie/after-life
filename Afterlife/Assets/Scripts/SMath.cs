using UnityEngine;

public static class SMath
{
	public static float RecursiveLerp(float current, float target, float epsilon, float delta)
	{
		return Mathf.LerpUnclamped(target, current, Mathf.Pow(epsilon, delta));
	}

	public static Vector2 RecursiveLerp(Vector2 current, Vector2 target, float epsilon, float delta)
	{
		return Vector2.LerpUnclamped(target, current, Mathf.Pow(epsilon, delta));
	}

	public static Vector3 RecursiveLerp(Vector3 current, Vector3 target, float epsilon, float delta)
	{
		return Vector3.LerpUnclamped(target, current, Mathf.Pow(epsilon, delta));
	}

	public static Quaternion RecursiveLerp(Quaternion current, Quaternion target, float epsilon, float delta)
	{
		return Quaternion.LerpUnclamped(target, current, Mathf.Pow(epsilon, delta));
	}
}
