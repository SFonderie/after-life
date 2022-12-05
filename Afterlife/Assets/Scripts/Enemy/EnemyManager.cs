/// <summary>
/// Manages Enemy Behavior scripts.
/// </summary>
public class EnemyManager : BehaviorManager<EnemyDelegate, EnemyContext>
{
	public override void OnDeath()
	{
		// Kill ourselves.
		Destroy(gameObject);
	}
}
