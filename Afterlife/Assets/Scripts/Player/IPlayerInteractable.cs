/// <summary>
/// Allows a script to react to player interactions.
/// </summary>
public interface IPlayerListener
{
	/// <summary>
	/// Invoked once each frame that a player hovers over this object.
	/// </summary>
	/// <param name="context">Player context from update.</param>
	public void OnHover(PlayerContext context);

	/// <summary>
	/// Invoked when the player interacts with this object.
	/// </summary>
	/// <param name="context">Player context from update.</param>
	public void OnStartInteract(PlayerContext context);

	/// <summary>
	/// Invoked when the player stops interacting with this object.
	/// Only invoked if the object itself uses the "Interacting" state.
	/// </summary>
	/// <param name="context">Player context from update.</param>
	public void OnStopInteract(PlayerContext context);

	/// <summary>
	/// Invoked to determine what verb should be used on the Interact text prompt.
	/// </summary>
	public string GetActionName();
}
