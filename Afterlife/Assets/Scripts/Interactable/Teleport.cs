using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IPlayerListener
{
    private string _actionName = "Teleport";

	/// <summary>
	/// This TP position to teleport.
	/// </summary>
    [SerializeField] Transform _finalPositionTP;

    GameObject player;

    private PlayerContext playerContext;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public string GetActionName()
    {
        return _actionName;
    }

    public void OnHover(PlayerContext context)
    {
        
    }

    public void OnStartInteract(PlayerContext context)
    {
        playerContext = context;
        Debug.Log("Starting teleport to: " + _finalPositionTP.position);
        player.GetComponent<CharacterController>().enabled = false;
        // Dispatch level sequence events.
        // ISceneListener.DispatchEvents(Listeners);
        playerContext.IsFadeOut = true;
    }

    public void OnStopInteract(PlayerContext context)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerContext)
        {
            if(playerContext.IsFadeLevelMax && playerContext.IsFadeOut) //change to full fade context var
            {
                player.transform.position = _finalPositionTP.position;
                player.GetComponent<CharacterController>().enabled = true;
                playerContext.IsFadeLevelMax = false;
            }
        }
    }
}
