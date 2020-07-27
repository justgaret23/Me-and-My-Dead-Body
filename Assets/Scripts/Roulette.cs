using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Roulette : MonoBehaviour
{
        private bool interactable;
        public float coins;
        public float coinCost;
        private int[] winnableNumbers = new int[36];   //Array of numbers that can result in victory

        void Awake()
        {

        }

        void FixedUpdate()
        {
            //Check if the player can interact with roulette
            if (interactable)
            {
                //If the player has enough coins to play roulette, start the game
                if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.playerCoins >= coinCost)
                {
                    //Play a game of roulette. All game procedures are within helper function playRoulette
                    Debug.Log("Roulette Start!");
                    PlayRoulette();
                }
                //If the player doesn't have enough coins to play roulette, tell them to find more coins
                else if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.playerCoins < coinCost)
                {
                    GameManager.Instance.interactable.SetText("Not enough coins! Please go look for coins elsewhere.");
                }
            }
        }
        
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Possessable"))
            {
                if (collision.gameObject.GetComponent<CharacterController2D>() && collision.gameObject.GetComponent<Possessable>().canInteract)
                {
                    GameManager.Instance.interactable.enabled = true;
                    //Text that tells player if they can interact
                    GameManager.Instance.interactable.SetText("Wanna play roulette? You can win up to 100 coins!");
                    interactable = true;
                }
            } else if (collision.gameObject.tag.Equals("Possessor"))
            {
                if (collision.gameObject.GetComponent<CharacterController2D>() && collision.gameObject.GetComponent<Possessor>().canInteract)
                {
                    GameManager.Instance.interactable.enabled = true;
                    interactable = true;
                }
            }

        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<CharacterController2D>())
            {
                interactable = false;
                GameManager.Instance.interactable.enabled = false;
                GameManager.Instance.interactable.SetText("");
            }
        }


        void PlayRoulette()
        {
            int rouletteNum;     //Number that the roulette lands on

            bool insideBet;  //Detects if bet user makes is an inside bet
            bool outsideBet; //Detects if bet user makes is an outside bet
            

            //prompt text: "What type of bet would you like to make?"
            
            //Inside
            //if (player picks inside)
            insideBet = true;
            int betNumber = 0;
            
            //Input number the player would like to bet on
            
            winnableNumbers[0] = betNumber;
            
            
            //Outside
            outsideBet = true;
            //Pick from one of the four bets:

            //Depending on the bet, certain numbers are flagged as being "win" numbers.
            //Numbers that you can win with are
            //If the bet was inside, you will receive a 100 coin payout
            //If the bet was outside, you will receive a 40 coin payout
            
            //Spin the roulette wheel (in this case, pick a random number between 1 and 36)
            rouletteNum = Random.Range(0, 36);

            //For each number in the winnableNumbers array
            for (int i = 0; i < winnableNumbers.Length; i++)
            {
                //If a number in the array equals a rouletteNum
                if (winnableNumbers[i] == rouletteNum)
                {
                    //Display text stating the player won
                    //If the player made an inside bet, add 100 coins
                    if (insideBet)
                    {
                        coins += 100;

                        //If the player made an outside bet, add 40 coins
                    } else if (outsideBet)
                    {
                        coins += 40;
                    }
                }
            }
            //If the number equals a "win" number, you receive the appropriate payout.
        }

        void PushOddNumbers()
        {
            //Odd number
            winnableNumbers[0] = 1;
            for (int i = 1; i <= 17; i++)
            {
                //Add all odd numbers
                winnableNumbers[i] = (2 * (i+1)) - 1;
            }
        }

        void PushEvenNumbers()
        {
            //Even number
            for (int i = 0; i <= 18; i++)
            {
                //Add all even numbers
                winnableNumbers[i] = 2*i;
            }
        }

        void PushBlackNumbers()
        {
            //Black number
            winnableNumbers[0] = 0;
            winnableNumbers[1] = 2;
            winnableNumbers[2] = 4;
            winnableNumbers[3] = 6;
            winnableNumbers[4] = 10;
            winnableNumbers[5] = 11;
            winnableNumbers[6] = 15;
            winnableNumbers[7] = 17;
            winnableNumbers[8] = 20;
            winnableNumbers[9] = 22;
            winnableNumbers[10] = 24;
            winnableNumbers[11] = 26;
            winnableNumbers[12] = 28;
            winnableNumbers[13] = 29;
            winnableNumbers[14] = 31;
            winnableNumbers[15] = 33;
            winnableNumbers[16] = 35;
            winnableNumbers[17] = 8;
        }

        void PushRedNumbers()
        {
            //Red number
            winnableNumbers[0] = 1;
            winnableNumbers[1] = 5;
            winnableNumbers[2] = 7;
            winnableNumbers[3] = 9;
            winnableNumbers[4] = 12;
            winnableNumbers[5] = 14;
            winnableNumbers[6] = 16;
            winnableNumbers[7] = 18;
            winnableNumbers[8] = 19;
            winnableNumbers[9] = 21;
            winnableNumbers[10] = 23;
            winnableNumbers[11] = 25;
            winnableNumbers[12] = 27;
            winnableNumbers[13] = 30;
            winnableNumbers[14] = 32;
            winnableNumbers[15] = 34;
            winnableNumbers[16] = 36;
            winnableNumbers[17] = 3;
        }
}

