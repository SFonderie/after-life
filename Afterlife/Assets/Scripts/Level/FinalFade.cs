using UnityEngine;

public class FinalFade : MonoBehaviour, ISceneListener
{
	[SerializeField, Tooltip("Panel used to jump to black.")]
	private GameObject VictoryPanel = null;

	[SerializeField, Tooltip("Parent for all victory panel objects.")]
	private GameObject VictoryContents = null;

	[SerializeField, Tooltip("Time it takes to finish the scene.")]
	private float MainDelay = 10;

	[SerializeField, Tooltip("Time after the jump to black before victory.")]
	private float BlackDelay = 2;

	private PlayerManager Manager = null;

	private bool Fired = false;

	private float SceneTime = 0;

	void Start()
	{
		GameObject Player = GameObject.FindGameObjectWithTag("Player");
		Manager = Player.GetComponent<PlayerManager>();

		VictoryPanel.SetActive(false);
		VictoryContents.SetActive(false);
	}

	void Update()
	{
		if (Fired)
		{
			Manager.Context.Interacting = true;

			SceneTime += Time.deltaTime;

			if (SceneTime > MainDelay + BlackDelay)
			{
				VictoryPanel.SetActive(true);
				VictoryContents.SetActive(true);
				Manager.Context.Winner = true;

				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
			}
			else if (SceneTime > MainDelay)
			{
				VictoryPanel.SetActive(true);
				VictoryContents.SetActive(false);
				Manager.Context.Winner = true;
			}
		}
	}

	public void OnPlayerEnter(Collider player)
	{

	}

	public void OnPlayerExit(Collider player)
	{

	}

	public void OnLevelSequence()
	{
		Fired = true;
		Debug.Log("STARTING FINAL SEQUENCE");
	}
}
