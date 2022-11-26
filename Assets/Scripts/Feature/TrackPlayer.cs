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
    [Tooltip("Effect when the bullet is flying.")] public GameObject MoveEffect;
    [Tooltip("Effect when the box explodes.")] public GameObject BoxExplosionEffect;

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

    public AudioClip MiniExplodeSound;
    private UnityEngine.Camera cam;
    
    void Start()
    {
        m_StartPosition = transform.position;
        m_Rotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        Player = GameObject.FindWithTag("Player");
        audioSource = gameObject.GetComponent<AudioSource>();
        explodeClip = Resources.Load<AudioClip>("Audio/explode");
        audioSource.clip = explodeClip;
        cam = UnityEngine.Camera.main;
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

                // Enable moving effect
                if (MoveEffect) {
                    MoveEffect.SetActive(true);
                }
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
        // Show explosion effect
        if (TeleportEffect)
        {
            Instantiate(TeleportEffect, transform.position, transform.rotation);
        }

        // Stop moving effect
        if (MoveEffect) {
            MoveEffect.SetActive(false);
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
            // Show box explosion effect
            if (BoxExplosionEffect) {
                Instantiate(BoxExplosionEffect, other.transform.position, other.transform.rotation);
                // Destroy(BoxExplosionEffect, 5f);
            }

            Destroy(other.gameObject);
            // DisplayMessage(msg);
            resetItem();
        }
        else if (other.gameObject.tag == "Cannon Receiver") {
            // Play explosion sound
            PlayMiniExplodeSound();
            
            if (MiniExplodeSound != null) {
                AudioSource.PlayClipAtPoint(MiniExplodeSound, 0.9f*Camera.main.transform.position + 0.1f*transform.position ,10f);
            }
            resetItem();
        }
    }

    private void DisplayMessage(string msg){
        var message = GameObject.FindGameObjectsWithTag("MessageCenter");
        if(message != null && message.Length > 0){
            message[0].GetComponent<Notification>().setMessage(msg);
        }
        
    }

    private void PlayMiniExplodeSound() {
        // Play explosion sound
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        // If the object is in the camera view
        if (MiniExplodeSound != null && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0) {
            AudioSource.PlayClipAtPoint(MiniExplodeSound, transform.position + 0.1f*transform.position, 10f);
        }
    }
}
