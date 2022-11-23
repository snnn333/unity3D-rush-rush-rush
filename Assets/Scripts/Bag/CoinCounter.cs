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
    public Item Diamond;
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
        diff = coinCount - lastCount;
    }

    void updateDisplay(){
        var text = GetComponent<TextMeshProUGUI>();
        text.text = coinCount.ToString();
    }

    void popUp(){
        if(popupText != null){
            if(diff > 0){
                var effect = Instantiate(popupText,transform.position, transform.rotation);
                effect.transform.SetParent(transform, false); 
                popupText.GetComponent<TextMeshProUGUI>().text = "+"+diff;
            }else if (diff < 0){
                var effect = Instantiate(popupText,transform.position, transform.rotation);
                effect.transform.SetParent(transform, false); 
                popupText.GetComponent<TextMeshProUGUI>().text = ""+diff;
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
