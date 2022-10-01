using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TrackPlayer : MonoBehaviour
{
    public GameObject Player = null;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Debug.Log("player found");
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null){
            var playerPosition = Player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        }
    }
}
