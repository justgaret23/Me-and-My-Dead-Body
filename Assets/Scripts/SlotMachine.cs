using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

/**
 * Code for slot machine
 */
public class SlotMachine : MonoBehaviour
{
    
    //Checks whether you can interact with a slot machine
    private bool interactable;
    
    //Checks how many coins you can win in a payout
    public float coins;
    
    //Checks how many coins it costs for a run at the slots
    public float coinCost;
    
    //Checks to see whether the slot animation is playing
    private bool vidPlaying;
    
    //Checks to see if the slot game was just played
    private bool justPlayed;
    
    //Checks to see if player won
    private bool win;

    //This slot machine will always win if true
    public bool alwaysWin;
    
    //Slot ints
    public int slot1;
    public int slot2;
    public int slot3;
    
    //Create hold
    private int hold = 150;
    private int UIhold = 150;
    private bool UIstay;

    /**
     * Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
     */
    private void FixedUpdate()
    {
        //Checks to see if player can interact with slot machine
        if (interactable)
        {
             //Checks to see if player is pressing space, has enough coins, and is not sitting through the vod
             if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.playerCoins >= coinCost && !vidPlaying)
             {
                    //Play slots video
                    GameManager.Instance.camera.transform.GetChild(0).gameObject.SetActive(true);
                    GameManager.Instance.camera.GetComponentInChildren<VideoPlayer>().Play();
                    GameManager.Instance.camera.GetComponentInChildren<VideoPlayer>().renderMode =
                        VideoRenderMode.CameraNearPlane;
                    
                    GameManager.Instance.interactable.SetText("Slot machine is currently running...");
                    
                    win = PlaySlots();
                    
                    //Set vidPlaying and justPlayed booleans to true
                    vidPlaying = true;
                    justPlayed = true;
             }
             //If the player doesn't have enough coins, they cannot play
                else if (GameManager.Instance.playerCoins < coinCost)
                {
                    GameManager.Instance.interactable.SetText("Not enough coins! Please go look for coins elsewhere.");
                }
                
                //If video has played and 
                if (justPlayed && vidPlaying)
                {
                    vidPlaying = GameManager.Instance.camera.GetComponentInChildren<VideoPlayer>().isPlaying;
                }

                if (justPlayed && !vidPlaying && hold > 0)
                {
                    hold -= 5;
                    if (hold < 0)
                    {
                        hold = 0;
                    }
                }

                if (justPlayed && !vidPlaying && hold == 0)
                {
                    GameManager.Instance.slot1.enabled = true;
                    GameManager.Instance.slot2.enabled = true;
                    GameManager.Instance.slot3.enabled = true;
                    GameManager.Instance.slotback.enabled = true;
                    GameManager.Instance.slot1.SetText(slot1.ToString());
                    GameManager.Instance.slot2.SetText(slot2.ToString());
                    GameManager.Instance.slot3.SetText(slot3.ToString());

                    //If the player won, they win the payout. If not, they lose the coins they bet
                    if (win)
                    {
                        GameManager.Instance.playerCoins += (int) coins;
                        GameManager.Instance.camera.GetComponentInChildren<VideoPlayer>().Stop();
                    }
                    else
                    {
                        GameManager.Instance.playerCoins -= (int) coinCost;
                        GameManager.Instance.camera.GetComponentInChildren<VideoPlayer>().Stop();
                    }

                    GameManager.Instance.camera.transform.GetChild(0).gameObject.SetActive(false);
                    justPlayed = false;
                    hold = 150;
                    UIstay = true;
                    UIhold = 150;
                }

                if (UIstay && UIhold > 0)
                {
                    UIhold--;
                    GameManager.Instance.interactable.SetText("Press esc to leave, or space to play again.");
                }
                else if (UIstay && UIhold <= 0 || Input.GetKeyDown(KeyCode.Escape))
                {
                    GameManager.Instance.slot1.enabled = false;
                    GameManager.Instance.slot2.enabled = false;
                    GameManager.Instance.slot3.enabled = false;
                    GameManager.Instance.slotback.enabled = false;
                    UIstay = false;
                    UIhold = 150;
                    GameManager.Instance.interactable.SetText("");
                }
        }
           
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the tag of the collided object of possessable
        if (collision.gameObject.tag.Equals("Possessable"))
        {
            if (collision.gameObject.GetComponent<CharacterController2D>() && collision.gameObject.GetComponent<Possessable>().canInteract)
            {
                GameManager.Instance.interactable.enabled = true;
                GameManager.Instance.interactable.SetText("Press Space to interact with Slot Machine. This will cost " + coinCost + " for a payout of " + coins + " coins.");
                interactable = true;
            }
        } else if (collision.gameObject.tag.Equals("Possessor"))
        {
            if (collision.gameObject.GetComponent<CharacterController2D>() && collision.gameObject.GetComponent<Possessor>().canInteract)
            {
                GameManager.Instance.interactable.enabled = true;
                GameManager.Instance.interactable.SetText("Press Space to interact with Slot Machine. This will cost " + coinCost + " for a payout of " + coins + " coins.");
                interactable = true;
            }
        }

    }

    /**
     * OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
     * This function can be a coroutine.
     */
    private void OnCollisionExit(Collision collision)
    {
        //If the gameObject collision is true
        if (collision.gameObject.GetComponent<CharacterController2D>())
        {
            //Make it uninteractable and delete text
            interactable = false;
            GameManager.Instance.interactable.enabled = false;
            GameManager.Instance.interactable.SetText("");
            
            //Get rid of slot picture
        }
    }
    
    private bool PlaySlots()
    {
        //Randomly generate slots
        slot1 = Random.Range(0, 6);
        slot2 = Random.Range(0, 6);
        slot3 = Random.Range(0, 6);

        if(alwaysWin){
            slot2 = slot1;
            slot3 = slot1;
        }
         
        //If all slots are equal to each other, return true
        return slot1 == slot2 && slot2 == slot3;
    }
}
