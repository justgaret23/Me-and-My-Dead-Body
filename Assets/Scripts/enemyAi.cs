using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * AI used for guards in casino level
 */
public class enemyAi : AI
{

    //Enemy speed (might be changed)
    public int enemySpeed = 4;

    //Starting position of enemy when game loads
    public Vector3 startingPosition;

    public override void Awake()
    {
        //Set playerDetected equal to false
        playerDetected = false;
        playerWasDetected = false;

        Debug.Log("patrolPOI Count: " + patrolPOI.Count);

        //currentSeek is set to 
        currentSeek = patrolPOI.First.Value;
        
        Debug.Log("Target: " + currentSeek.position);
        
        //Set transform position equal to the starting position inputted in Unity
        this.transform.position = startingPosition;

    }

    /**
     * Enemy AI that executes when player is noticed
     */
    public override void ExecuteOnPlayerDetection()
    {
        //Set currentSeek equal to the player
        currentSeek = GameManager.GetComponent<GameManager>().player.transform;
        
        //Debug statement that confirms code is running
        Debug.Log("Moving to player! Current position: " + transform.position);
    }
}
