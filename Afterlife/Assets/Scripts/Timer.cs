using UnityEngine;

/// <summary>
/// Tracks time relative to a starting point.
/// </summary>
public struct Timer
{
	/// <summary>
	/// Time at last reset.
	/// </summary>
	private float _timeStart;

	/// <summary>
	/// Offsets local time to affect Timer completion.
	/// </summary>
	private float _timeOffset;

	/// <summary>
	/// Timer has ended, or was forced to end.
	/// </summary>
	private bool _hasEnded;

	/// <summary>
	/// Time after which the Timer elapses.
	/// </summary>
	public float Duration { get; private set; }

	/// <summary>
	/// Actual time plus any Timer offset.
	/// </summary>
	public float LocalTime => Time.time + _timeOffset;

	/// <summary>
	/// Absolute time since Timer start.
	/// </summary>
	public float TimeSinceStart => LocalTime - _timeStart;

	/// <summary>
	/// Number of effective elapses since the timer's start.
	/// </summary>
	public float TimerCompletions => TimeSinceStart / Duration;

	/// <summary>
	/// Time since the timer started in seconds.
	/// </summary>
	public float Progress => HasElapsed ? TimeSinceStart : Duration;

	/// <summary>
	/// Percent of timer duration since the timer started.
	/// </summary>
	public float RelativeProgress => Progress / Duration;

	/// <summary>
	/// Time until the timer elapses in seconds.
	/// </summary>
	public float Remainder => Duration - Progress;

	/// <summary>
	/// Percent of timer duration until the timer elapses.
	/// </summary>
	public float RelativeRemainder => Remainder / Duration;

	/// <summary>
	/// Whether the timer has elapsed.
	/// </summary>
	public bool HasElapsed => _hasEnded || _timeStart + Duration < LocalTime;

	/// <summary>
	/// Sets and starts the timer.
	/// </summary>
	/// <param name="duration">Timer duration.</param>
	public void Set(float duration)
	{
		Duration = duration;
		_timeStart = Time.time;
		_timeOffset = 0f;
		_hasEnded = false;
	}

	/// <summary>
	/// Forces the timer to elapse.
	/// </summary>
	public void End() => _hasEnded = true;

	/// <summary>
	/// Offsets local time, which can speed or slow the timer.
	/// </summary>
	/// <param name="offset">Offset to apply to the timer.</param>
	public void OffsetLocalTime(float offset) => _timeOffset += offset;
}
