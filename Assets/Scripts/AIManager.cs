using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * General AI manager for all instances of AI in game
 */
public class AIManager : MonoBehaviour
{
    
    //Create an instance of the AIManager
    public static AIManager instance;
    
    //LinkedList of AIs
    public LinkedList<AI> AICollection = new LinkedList<AI>();
    
    //LinkedList of NPC Points of Interest
    public LinkedList<Transform> npcPOI = new LinkedList<Transform>();
    
    //LinkedList of enemy Points of Interest
    public LinkedList<Transform> enemyPOI = new LinkedList<Transform>();
    private void Awake()
    {
        //If prior instance doesn't exist, create an instance.
        if (instance == null)
        {
            instance = this;
        }
        //If the instance is different, destroy the current instance
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        
        //Instance will remain constant between scenes
        DontDestroyOnLoad(instance);
        
        //Array of npc points and enemy points are provided for the AI to revolve around
        GameObject[] npcPoints = GameObject.FindGameObjectsWithTag("npcPOI");
        GameObject[] enemyPoints = GameObject.FindGameObjectsWithTag("enemyPOI");
        
        //An array of AIs are initialized for both enemies and NPCs
        AI[] npcAI = FindObjectsOfType<npcAi>();
        AI[] enemyAI = FindObjectsOfType<enemyAi>();
        
        Debug.Log("Amount of enemy points: " + enemyPoints.Length);
        Debug.Log("Amount of npc points: " + npcPoints.Length);
        
        //For each point in the array of NPC points, add the transform element of the object to the LinkedList of transforms
        for (int i = 0; i < npcPoints.Length; i++)
        {
            npcPOI.AddLast(npcPoints[i].transform);
        }
        
        //For each point in the array of AIs
        for (int i = 0; i < npcAI.Length; i++)
        {
            //Add it to the LinkedList of AIs
            AICollection.AddLast(npcAI[i]);
            
            //The patrolPOI of the AI is set equal to (!!! This might cause issues !!!)
            npcAI[i].patrolPOI = npcPOI;
        }

        
        //For each point in the array of enemy points, add the transform element of the object to the LinkedList of transforms
        for (int i = 0; i < enemyPoints.Length; i++)
        {
            enemyPOI.AddLast(enemyPoints[i].transform);
        }
        
        //For each point in the array of AIs
        for (int i = 0; i < enemyAI.Length; i++)
        {
            //Add it to the LinkedList of AIs
            AICollection.AddLast(enemyAI[i]);
            
            //The patrolPOI of the AI is set equal to (!!! This might cause issues !!!)
            enemyAI[i].patrolPOI = enemyPOI;
        }
    }
}
