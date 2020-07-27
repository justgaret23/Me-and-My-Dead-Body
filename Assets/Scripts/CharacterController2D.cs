using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CharacterController2D : MonoBehaviour
{
    //Float that controls character speed
    public float speed;
    
    //Instance of camera object
    public GameObject camera;
    
    //Distance between camera and player
    private static Vector3 distance;
    
    //Player ghost form
    public GameObject GhostForm;
    
    //Possession cooldown period
    private bool waitingforQ;
    private int QTimer = 25;

    private void Start()
    {
        //Calculate distance between camera and object
        distance =  camera.transform.position - gameObject.transform.position;
    }
    private void FixedUpdate()
    {
        //Current camera position
        camera.transform.position = this.gameObject.transform.position + distance;
        this.gameObject.transform.rotation = new Quaternion(0,0,0,0);
        //Allows ghost to enter zombie initially
        TickDownQ();
        if (Input.GetMouseButtonDown(1) && !waitingforQ)
        {
            waitingforQ = true;
        }
        else if (Input.GetMouseButtonDown(1) && waitingforQ)
        {
            Possessed(gameObject.GetComponent<Possessor>().firstSpawn);
            
        }
    }
    private void Update()
    {
        //movement controls
        Collider controller = this.gameObject.GetComponent<Collider>();
        Vector2 leftR = transform.TransformDirection(Vector3.right);
        var horSpeed = speed * Input.GetAxis("Horizontal");
        Vector2 upD = transform.TransformDirection(Vector3.up);
        var upwardSpeed = speed * Input.GetAxis("Vertical");
        controller.attachedRigidbody.velocity = new Vector3(-2 * horSpeed, 0, -2 * upwardSpeed);
        if (this.gameObject.tag.Equals("Possessable"))
        {
            if (controller.attachedRigidbody.velocity.x < 0 && !this.gameObject.GetComponent<Possessable>().flipped || controller.attachedRigidbody.velocity.x > 0 && this.gameObject.GetComponent<Possessable>().flipped)
            {
                this.gameObject.GetComponent<Transform>().localScale = new Vector3(-1 * (this.gameObject.GetComponent<Transform>().localScale.x), (this.gameObject.GetComponent<Transform>().localScale.y), (this.gameObject.GetComponent<Transform>().localScale.z));
                this.gameObject.GetComponent<Possessable>().flipped = !this.gameObject.GetComponent<Possessable>().flipped;
            }
        } else if (this.gameObject.tag.Equals("Possessor"))
        {
            if (controller.attachedRigidbody.velocity.x < 0 && !this.gameObject.GetComponent<Possessor>().flipped || controller.attachedRigidbody.velocity.x > 0 && this.gameObject.GetComponent<Possessor>().flipped)
            {
                this.gameObject.GetComponent<Transform>().localScale = new Vector3(-1 * (this.gameObject.GetComponent<Transform>().localScale.x), (this.gameObject.GetComponent<Transform>().localScale.y), (this.gameObject.GetComponent<Transform>().localScale.z));
                this.gameObject.GetComponent<Possessor>().flipped = !this.gameObject.GetComponent<Possessor>().flipped;
            }
        }
        
        //On right mouse click, exit whatever object you are currently possessing and enter ghost form 
        if (Input.GetMouseButtonDown(1) && this.gameObject.tag.Equals("Possessable"))
        {
            EndPossession();
        } 

        //On left mouse click in ghost form, possess whatever object you are currently pointing towards
        if (Input.GetMouseButtonDown(0) && this.gameObject.tag.Equals("Possessor"))
        {
            Possessed(GameManager.Instance.selectedPossessable);
        }
    }

    /**

    */
    private void TickDownQ()
    {
        if (waitingforQ)
        {
            QTimer--;
            if (QTimer <= 0)
            {
                QTimer = 25;
                waitingforQ = false;
            }
        }
    }

    private void Possessed(GameObject target)
    {
        
        if (this.gameObject.tag.Equals("Possessor") && target.gameObject.tag.Equals("Possessable"))
        {
            //Collider is disabled
            this.GetComponent<Collider>().enabled = false;

            //player position is set equal to target transform position
            this.transform.position = target.transform.position;

            //Instantiate in ghost sprite
            CharacterController2D g = target.AddComponent<CharacterController2D>();

            //Assign current ghost attributes to the object
            g.GhostForm = this.GhostForm;
            g.camera = this.camera;

            //Disable Rigidbody constraints that non-player objects typically us
            target.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            //Speed is set to speed of the target
            g.speed = target.GetComponent<Possessable>().speed;

            //Camera is centered behind the possessed object
            g.camera.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + 122, g.transform.position.z + 130);

            //Set sprite equal to possessed sprite
            target.gameObject.GetComponent<SpriteRenderer>().sprite =
                target.gameObject.GetComponent<Possessable>().possessedSprite;

            //Play possess animation
            this.GetComponent<Animator>().Play("Scale");

            //Destroy ghost component
            Destroy(this.gameObject,1);

            //Player is set equal to target
            GameManager.Instance.player = target;

            //List of CharacterController2D duplicates is created and all objects within it are destroyed
            CharacterController2D[] dupes = FindObjectsOfType<CharacterController2D>();
            for (int i = 0; i < dupes.Length; i++)
            {
                if (dupes[i].gameObject != GameManager.Instance.player)
                {
                    Destroy(dupes[i]);
                }
            }
        }
    }

    /**
     * Code that causes the ghost to leave whatever object it is currently possessing
     */
    private void EndPossession()
    {
        if (this.gameObject.tag.Equals("Possessable"))
        {
            //Vector3 dx is initialized
            Vector3 dX;
            if (this.gameObject.GetComponent<Possessable>().flipped)
            {
                 dX = this.transform.position - new Vector3(this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x * 2, 0, 0);
            }
            else
            {
                 dX = new Vector3(this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x * 2, 0, 0) + this.transform.position;
            }
           
            //Set Quaternion rX equal to the rotation of the current object
            Quaternion rX = this.transform.rotation;
            
            //If
            if (GameManager.Instance.player.tag.Equals("Possessable"))
            {
                GameObject ghost = Instantiate(GhostForm, dX, rX);
                GameManager.Instance.player = ghost;
            }

            //List of CharacterController2Ds
            CharacterController2D[] dupes = FindObjectsOfType<CharacterController2D>();
            for (int i = 0; i < dupes.Length; i++)
            {
                if (dupes[i].gameObject != GameManager.Instance.player)
                {
                    Destroy(dupes[i]);
                }
            }

            GameManager.Instance.player.GetComponent<Animator>().Play("Scale Up");
            CharacterController2D g = GameManager.Instance.player.AddComponent<CharacterController2D>();
            
            //Set ghostForm equal to the current GhostForm object
            g.GhostForm = GhostForm;
            
            //Set ghost camera settings equal to the current camera
            g.camera = camera;
            
            //Recenter camera to the ghost's position
            g.camera.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + 122, g.transform.position.z + 130);
            
            //Set speed of the ghost
            g.speed = 12.0f;
            
            //Set rigidbody constraints to the current gameObject
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            
            //Render the ghost sprite
            gameObject.GetComponent<SpriteRenderer>().sprite =
                gameObject.GetComponent<Possessable>().nonPossessedSprite;

            //Get rid of CharacterController2D component
            Destroy(gameObject.GetComponent<CharacterController2D>());

            //Nullify instance of selectedPossessable
            GameManager.Instance.selectedPossessable = null;
        }
       
    }
}