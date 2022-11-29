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
	/// Invoked each time that the player interacts with this object.
	/// </summary>
	/// <param name="context">Player context from update.</param>
	public void OnInteraction(PlayerContext context);
}
