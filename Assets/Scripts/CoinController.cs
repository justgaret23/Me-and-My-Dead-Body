using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //For each frame, update the player's total coins (might want to put this in an accumulatory if statement)
        this.gameObject.GetComponent<TextMeshProUGUI>().SetText(GameManager.Instance.playerCoins.ToString());
    }
}
