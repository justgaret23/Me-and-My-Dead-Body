using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * AI used for NPCs
 */
public class npcAi : AI
{
    
    public override void Awake()
    {
        //Set playerDetected equal to false
        playerDetected = false;
        playerWasDetected = false;

        Debug.Log("patrolPOI Count: " + patrolPOI.Count);
        
        //currentSeek is set to 
        currentSeek = patrolPOI.First.Value;
    }
    
    public override void ExecuteOnPlayerDetection()
    {
        currentSeek = GameManager.GetComponent<GameManager>().player.transform;
        Debug.Log("Moving to player");
    }
}
