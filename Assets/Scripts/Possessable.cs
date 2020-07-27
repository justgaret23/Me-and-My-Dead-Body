using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Possessable : MonoBehaviour
{
    //Speed of possessable object
    public float speed;
    
    //Checks to see if you can interact with the possessable object
    public bool canInteract;
    
    //Sprite of possessable object when it's not currently possessed
    public Sprite nonPossessedSprite;
    
    //Sprite of possessable object when it is currently being possessed
    public Sprite possessedSprite;
    
    //Text that indicates whether an object is presentable
    public TextMeshProUGUI text;
    
    //Boolean flipped
    public bool flipped;
    
    //Cursor texture
    public Texture2D cursor;
    // Start is called before the first frame update
    private void Start()
    {
        //check to see if collision boxes are overlapping through canInteract
        if (canInteract == null)
        {
            canInteract = false;
        }
    }
    
    /**
     * OnMouseDown is called when the user has pressed the mouse button while over the Collider. This function can be a coroutine.
     */
    private void OnMouseDown()
    {
        //Possess the object
        if (GameManager.Instance.possessableObjects.Contains(this.gameObject))
        {
            GameManager.Instance.selectedPossessable = this.gameObject;
        }
    }

    /**
     * Called every frame while the mouse is over the Collider. This function can be a coroutine.
     */
    private void OnMouseOver()
    {
        //Enable text
        if (text.enabled == false)
        {
            text.enabled = true;
        }
        
        //Enable cursor texture
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        
        //Enable TextMeshPro stating that an object is possessable
        text.SetText("Click to Possess " + this.gameObject.name + " .");

        //Display unique text is the gameObject is the body
        if (gameObject.name == "Zombie")
        {
            text.SetText("Click to return to your body.");
        }
    }

    /**
     * Called when the mouse is not any longer over the Collider. This function can be a coroutine.
     */
    private void OnMouseExit()
    {
        //Disable text
        if (text.enabled)
        {
            text.enabled = false;
        }
        
        //Get rid of cursor texture
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        
        //Eliminate current text buffer
        text.SetText("");
    }
 }
