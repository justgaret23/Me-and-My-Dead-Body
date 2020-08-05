using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AI : MonoBehaviour
{
    //Create instance of GameManager
    public GameObject GameManager;
    
    //Boolean that keeps track of whether player is detected
    public bool playerDetected;
    
    //Boolean that checks if the enemy or npc remembers the player
    public bool playerWasDetected;
    
    //Amount of time the enemy will be alert of the player's presence after the player escapes detection
    public readonly int rememberPlayerTimer;

    //Initialized rememberPlayerTicker
    private int rememberPlayerTicker = 0;

    //LinkedList of patrol points of interest
    public LinkedList<Transform> patrolPOI = new LinkedList<Transform>();

    //point an object controlled by AI is currently going towards
    protected Transform currentSeek;


    /**
     * Abstract method Awake - initializes differently depending on which type of AI is being called
     */
    public abstract void Awake();

    void FixedUpdate()
    {
        //Check the following:
        // - Was the player previously detected?
        // - Is the player currently detected?
        // - Is the rememberPlayerTicker more than zero?
        if (playerWasDetected && !playerDetected && rememberPlayerTicker >= 0)
        {
            //If the rememberPlayerTicker is more than zero, decrease it
            //Otherwise, set it equal to the rememberPlayerTicker and make the enemy forget the player
            if (rememberPlayerTicker >= 0 )
            {
                //Decrease counter once per frame
                rememberPlayerTicker--;
            } else
            {
                rememberPlayerTicker = rememberPlayerTimer;
                playerWasDetected = false;
            }
        } 
        
        //If the player isn't detected and enemy doesn't remember player, execute typical patrol route code
        //Else, run the special code that operates upon player detection
        if (!playerDetected && !playerWasDetected)
        {
            PatrolRoute();
        }
        else if(playerDetected || playerWasDetected)
        {
            ExecuteOnPlayerDetection();
        }
    }

    /**
     * Abstract method - different conditions are executed by AI upon player detection
     */
    public abstract void ExecuteOnPlayerDetection();

    /**
     * Method that dictates usual AI behavior
     */
    public void PatrolRoute()
    {
        //If the current target is itself

        /* CODEBLOCK FOR LOOPING GUARD POINTS ~ Varun(Cashmere)
         * 
         * The issue was the source and destination were being compared using '==' which doesn't always work
           due to floating point inaccuracies
                                         
            > To solve this, the bool function V3Equal(V3 a, V3b) compares for equality in squared magnitude to a precision of 0.0001
            > Function V3Equal obtained from https://answers.unity.com/questions/395513/vector3-comparison-efficiency-and-float-precision.html
            > What happens here is it squares the magnitudes of the V3's and takes their difference. If the difference is less than
              0.0001, it returns true which loops the positions in the list.
         */

        if (V3Equal(currentSeek.position, this.gameObject.transform.position))
        {
            
            //Delete the currentPOI
            patrolPOI.Remove(patrolPOI.Find(currentSeek).Value);
            
            //Add it again at the end of the list
            patrolPOI.AddLast(currentSeek);
            
            //Set currentSeek equal to the first value
            currentSeek = patrolPOI.First.Value;

        }
        
        //Set destination as currentSeek.position
        //this.gameObject.GetComponent<NavMeshAgent>().SetDestination(currentSeek.position);

        //Set a transformation vector
        this.transform.position =
            Vector3.MoveTowards(this.transform.position, currentSeek.position, 5 * Time.deltaTime);

    }

    // See PatrolRoute() for more information on V3Equal(V3 a, V3b)
    
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the collision of the AI overlaps with that of the player, player is detected
        if (collision.collider.transform.parent.Equals(GameManager.GetComponent<GameManager>().player))
        {
            playerDetected = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Once the player gets out of range, they are no longer detected but playerWasDetected is set to true, starting the countdown timer
        if (collision.collider.transform.parent.Equals(GameManager.GetComponent<GameManager>().player))
        {
            playerDetected = false;
            playerWasDetected = true;
            rememberPlayerTicker = rememberPlayerTimer;
        }
    }
}
