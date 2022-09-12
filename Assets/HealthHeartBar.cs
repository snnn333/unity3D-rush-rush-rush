using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject Heart;
    public Health Health;
    List <HealthHeart> hearts = new List<HealthHeart>();
    // List <HealthHearts> hearts = new List<HealthHearts>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Health.maxHealth; i++)
        {
            GameObject heart = Instantiate(Heart, transform);
            hearts.Add(heart.GetComponent<HealthHeart>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < Health.currentHealth)
            {
                hearts[i].SetHeartImage(HeartStatus.Full);
            }
            else
            {
                hearts[i].SetHeartImage(HeartStatus.Empty);
            }
        }
    }
    public void CreateEmptyHearts()
    {
        
        GameObject heart = Instantiate(Heart, transform);
        HealthHeart heartScript = heart.GetComponent<HealthHeart>();
        heartScript.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartScript);
        
    }  
    public void DrawHearts()
    {
        for (int i = 0; i < Health.maxHealth; i++)
        {
            if (i < Health.currentHealth)
            {
                hearts[i].SetHeartImage(HeartStatus.Full);
            }
            else
            {
                hearts[i].SetHeartImage(HeartStatus.Empty);
            }
        }
    }
    public void ClearHearts()
    {
        foreach (HealthHeart heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();
    }
}
