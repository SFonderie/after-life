using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, ISceneListener
{
	/// <summary>
	/// Current scene.
	/// </summary>
    Scene _scene;

	/// <summary>
	/// Number of scenes in the build.
	/// </summary>
    int _numberOfScenes;

	/// <summary>
	/// Index of next scene.
	/// </summary>
    int _nextSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        _scene = SceneManager.GetActiveScene();
        _numberOfScenes = SceneManager.sceneCountInBuildSettings;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLevelSequence()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerEnter(Collider player)
    {
        if(_scene.buildIndex >=_numberOfScenes){
            _nextSceneIndex = 0;
        }
        else{
            _nextSceneIndex = _scene.buildIndex + 1;
        }
        SceneManager.LoadScene(_nextSceneIndex);
    }

    public void OnPlayerExit(Collider player)
    {
        Debug.Log("Player left");
    }

}
