using UnityEngine;

/// <summary>
/// Scriptable object for holding player settings.
/// </summary>
[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game Data/Player Settings")]
public class PlayerSettings : ScriptableObject
{
	/// <summary>
	/// Look sensitivity for pointer inputs like mice.
	/// </summary>
	[Header("Camera Settings"), Min(0f), Tooltip("Look sensitivity for pointer inputs like mice.")]
	public float SensitivityPointer = 1f;

	/// <summary>
	/// Look sensitivity for normalized inputs like gamepads.
	/// </summary>
	[Min(0f), Tooltip("Look sensitivity for normalized inputs like gamepads.")]
	public float SensitivityNormal = 50f;

	/// <summary>
	/// Invert vertical camera movement?
	/// </summary>
	[Tooltip("Invert vertical camera movement?")]
	public bool InvertCameraY = false;
}
