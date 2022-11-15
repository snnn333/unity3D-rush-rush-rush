using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class TrackPlayer : MonoBehaviour
{
    public GameObject Player = null;
    public float speed = 2f;
    public Vector3 m_StartPosition;
    public float follow_distance = 15f;
    // Stop tracking the player within this distance, so player can take action to dodge the bullet
    public float lifeTime = 10f;
    public float waitTime = 2f;

    [Tooltip("Effect to start teleport.")] public GameObject TeleportEffect;

    private Vector3 m_Direction;
    private Quaternion m_Rotation;
    // Whether the bullet is tracking the player
    private bool isTracking = false;
    // Whether the bullet is moving or sleeping
    private bool isMoving = true;

    public string msg = "Box destroyed by cannon!";
    // public Enums.Directions useSide = Enums.Directions.Up;
    // Start is called before the first frame update
    
    public AudioSource audioSource;
    public AudioClip explodeClip;
    
    void Start()
    {
        m_StartPosition = transform.position;
        m_Rotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        Player = GameObject.FindWithTag("Player");
        audioSource = gameObject.GetComponent<AudioSource>();
        explodeClip = Resources.Load<AudioClip>("Audio/explode");
        audioSource.clip = explodeClip;
    }

    IEnumerator WaitThenDie()
    {
        // The bullet can survive at most the life time, afterward the bullet will reset to the original position
        yield return new WaitForSeconds(lifeTime + waitTime);
        resetItem();
    }

    void FixedUpdate()
    {
        if(Player != null){
            var playerPosition = Player.transform.position;
            float playerBulletDistance = Vector3.Distance(transform.position,playerPosition);

            if (isMoving == false) {
                return;
            }

            // When the player is whithin the follow distance, start to track the player
            if (isTracking == false && playerBulletDistance < follow_distance && Math.Abs(transform.position.y - playerPosition.y) < 2) {
                isTracking = true;
                StartCoroutine(WaitThenDie());
            }

            // When the player is still whithin the follow distance, continue to track the player
            if(isTracking == true && playerBulletDistance < follow_distance){
                Vector3 DestPosition = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, DestPosition, speed * Time.deltaTime);
                transform.LookAt(DestPosition); 
            }
            
            // Stop tracking the player after the player is outside of the follow radius
            if (isTracking == true && playerBulletDistance >= follow_distance){
                transform.Translate(transform.forward*speed*Time.deltaTime, Space.World);
            }
            
        }
    }

    IEnumerator WaitThenLaunch()
    {
        // Before launching the bullet, sleep for some time
        isMoving = false;
        yield return new WaitForSecondsRealtime(waitTime);
        isMoving = true;

    }

    private void resetItem(){
        if (TeleportEffect)
            {
                Instantiate(TeleportEffect, transform.position, transform.rotation);
            }
        isTracking = false;
        StopAllCoroutines();
        transform.position = m_StartPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        StartCoroutine(WaitThenLaunch());
        StartCoroutine(WaitThenDie());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Destroyable"){
            Debug.Log("destroy object");
            audioSource.Play();
            Destroy(other.gameObject);
            DisplayMessage(msg);
            resetItem();
        }
        else if (other.gameObject.tag == "Cannon Receiver") {
            resetItem();
        }
    }

    private void DisplayMessage(string msg){
        var message = GameObject.FindGameObjectsWithTag("MessageCenter");
        if(message != null && message.Length > 0){
            message[0].GetComponent<Notification>().setMessage(msg);
        }
        
    }
}
