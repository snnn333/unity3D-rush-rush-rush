using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    public AudioClip coinSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(coinSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position ,10f);
            // AudioSource audio = GetComponent<AudioSource>();
            // audio.Play();
        }
    }
}
