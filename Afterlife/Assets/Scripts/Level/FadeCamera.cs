using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Slides a door open or closed via a lerp between transforms.
/// </summary>
public class FadeCamera : PlayerDelegate
{
	/// <summary>
	/// Fade out/in UI is off by default. Player will turned it on with a level sequence when using tp.
	/// </summary>
	// private bool _isFadeOn = false;

	// /// <summary>
	// /// Time it takes for the camera to fade in and out.
	// /// </summary>
	// [Range(0.05f, 1f), SerializeField, Tooltip("Time it takes for the door to open or close.")]
	// private float FadeTime = 0.25f;

	/// <summary>
	/// Current interpolation factor.
	/// </summary>
	float Interpolation = 0;
	/// <summary>
	/// Door slide curve.
	/// </summary>
	[SerializeField, Tooltip("Door slide curve.")]
	private AnimationCurve FadeCurve = null;

    

	/// <summary>
	/// Time it takes for the camera to fade out.
	/// </summary>
	[Range(0.05f, 1f), SerializeField, Tooltip("Time it takes for the camera to fade out.")]
    float _curveTime;

    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _curveTime = FadeCurve[FadeCurve.length - 1].time;
    }

    void Update()
    {
    }

    public void OnPlayerEnter(Collider player)
    {
        
    }

    public void OnPlayerExit(Collider player)
    {
        
    }

    public override void HandleInput(InputAction.CallbackContext context)
    {
        
    }

    public override void UpdateDelegate(PlayerContext context)
    {
		if (context.IsFadeOut)
		{
			float increment = Time.deltaTime / _curveTime;
			Interpolation += increment;
			Interpolation = Mathf.Clamp(Interpolation, 0, 1);

            var tempColor = _image.color;
            tempColor.a = FadeCurve.Evaluate(Interpolation);
			_image.color = tempColor;

            if(FadeCurve.Evaluate(Interpolation) >= 1)
            {
                context.IsFadeLevelMax = true;
            }

            if(Interpolation >= _curveTime)
            {
                context.IsFadeOut = false;
            }
		}
    }
}