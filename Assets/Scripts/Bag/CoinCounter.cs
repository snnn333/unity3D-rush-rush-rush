using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CoinCounter : MonoBehaviour
{
    // ATTACH THIS TO COIN DISPLAY TEXT
    public Bag bag;
    public GameObject popupText;
    public Item Coin;

    public int TotalDiamond = 0;    // The total amount of diamond in the level
    private int coinCount = 0;
    private int lastCount = 0;

    private int diff = 0;
    
    void updateCount(){
        lastCount = coinCount;

        if(bag != null){
            if(bag.ContainsItem(Coin)){
                coinCount = Coin.num;
            }else{
                coinCount = 0;
            }
        }
    }

    void updateDisplay(){
        var text = GetComponent<TextMeshProUGUI>();

        text.text = coinCount.ToString();

        // Append the total diamond to the diamond count
        if (Coin.name == "Diamond") {
            text.text += "/" + TotalDiamond;
        }
    }

    void popUp(){
        if(popupText != null){
            diff = coinCount - lastCount;

            if(diff > 0){
                popupText.GetComponent<TextMeshProUGUI>().text = "+"+diff.ToString();
                var effect = Instantiate(popupText,transform.position, transform.rotation);
                effect.transform.SetParent(transform, false); 
            }else if (diff < 0){
                popupText.GetComponent<TextMeshProUGUI>().text = ""+diff.ToString();
                var effect = Instantiate(popupText,transform.position, transform.rotation);
                effect.transform.SetParent(transform, false); 
            }
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        updateCount();
        updateDisplay();
        popUp();
    }
}
