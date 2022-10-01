using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthHeart : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite fullHeart, emptyHeart;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();

    }
    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
        }
    }
}

public enum HeartStatus
{
    Full = 1,
    Empty = 0
}
