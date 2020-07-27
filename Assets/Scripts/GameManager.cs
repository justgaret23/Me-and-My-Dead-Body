using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour

{
    //Create instance of GameManager
    public static GameManager Instance;

    //Checks whether the player can interact with a given object
    public bool playerInteractable;

    //Camera
    public GameObject camera;

    //Coins the player currently has
    public int playerCoins;

    //GameObject of the player
    public GameObject player;

    //List of interactable objects
    public List<GameObject> interactableObjects;

    //Array of possessable objects
    public GameObject[] possessableObjects;

    public TextMeshProUGUI interactable;
    public GameObject currentlyInteract = null;
    public GameObject selectedPossessable = null;

    //Slot results of all three slots
    public TextMeshProUGUI slot1;
    public TextMeshProUGUI slot2;
    public TextMeshProUGUI slot3;

    //Slot machine image
    public Image slotback;


    /**
     * Awake is called when the script instance is being loaded.
     */
    private void Awake()
    {
        //If an instance of the GameManager doesn't already exist, make it 
        if (Instance == null)
        {
            Instance = this;
        }
        //If the instance is not correct, delete it
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        //Preserves GameManager between scenes
        DontDestroyOnLoad(Instance);

        //Set possessableObjects equal to all the game objects with the tag possessable
        possessableObjects = GameObject.FindGameObjectsWithTag("Possessable");
        for (int i = 0; i < possessableObjects.Length; i++)
        {
            //If a possessable object contains the CharacterController2D component
            if (possessableObjects[i].GetComponent<CharacterController2D>())
            {
                //Set sprite equal to possessed sprite
                possessableObjects[i].GetComponent<SpriteRenderer>().sprite =
                    possessableObjects[i].GetComponent<Possessable>().possessedSprite;
                
                //
                possessableObjects[i].GetComponent<CharacterController2D>().GhostForm.GetComponent<Possessor>()
                    .firstSpawn = possessableObjects[i];
            }
            else
            {
                //Set sprite equal to non-possessed sprite
                possessableObjects[i].GetComponent<SpriteRenderer>().sprite =
                    possessableObjects[i].GetComponent<Possessable>().nonPossessedSprite;
                
                //Treat it like a typical object in the game's code
                possessableObjects[i].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        //Disable all slots and slot screen
        slot1.enabled = false;
        slot2.enabled = false;
        slot3.enabled = false;
        slotback.enabled = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
        interactable.enabled = false;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       
        for (int i = 0; i < interactableObjects.Count; i++)
        {

        }

    }
}
