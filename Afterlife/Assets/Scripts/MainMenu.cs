using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField, Tooltip("Loading bar slider object.")]
	private Slider LoadingBar = null;

	void Start()
	{
		LoadingBar.gameObject.SetActive(false);

		// Makes sure that time resumes if we come from a pause menu.
		Time.timeScale = 1;
	}

	public void OnPlay()
	{
		LoadingBar.gameObject.SetActive(true);
		StartCoroutine(LoadScene());
	}

	public void OnQuit()
	{
		Application.Quit();
	}

	IEnumerator LoadScene()
	{
		Scene scene = SceneManager.GetActiveScene();
		AsyncOperation loader = SceneManager.LoadSceneAsync(scene.buildIndex + 1);

		while (!loader.isDone)
		{
			if (LoadingBar)
			{
				LoadingBar.value = loader.progress;
			}

			yield return null;
		}
	}
}
