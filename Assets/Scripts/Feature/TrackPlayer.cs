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
    public Vector3 m_StartPosition;
    public float follow_distance = 15f;
    // Stop tracking the player within this distance, so player can take action to dodge the bullet
    public float stop_follow_distance = 5f;
    public float lifeTime = 10f;
    public float waitTime = 2f;
    private Vector3 m_Direction;
    private Quaternion m_Rotation;
    // Whether the bullet is tracking the player
    private bool isTracking = true;

    public string msg = "Wall destroyed by cannon!";
    // public Enums.Directions useSide = Enums.Directions.Up;
    // Start is called before the first frame update
    void Start()
    {
        m_StartPosition = transform.position;
        m_Rotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        Player = GameObject.FindWithTag("Player");

        StartCoroutine(waiter());
        StartCoroutine(WaitThenDie());
    }

    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(lifeTime);
        resetItem();
    }



    // Update is called once per frame
    void Update()
    {
        if(Player != null){
            var playerPosition = Player.transform.position;
            float playerBulletDistance = Vector3.Distance(transform.position,playerPosition);

            if (playerBulletDistance < stop_follow_distance) {
                Debug.Log("Bullet is too close to the player. Stop tracking the player");
                isTracking = false;
            }
            
            if(isTracking && playerBulletDistance < follow_distance){
                Vector3 DestPosition = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, DestPosition, speed * Time.deltaTime);
                transform.LookAt(DestPosition); 
            }else{
                transform.Translate(transform.forward*speed*Time.deltaTime, Space.World);
            }
            
        }
    }

    IEnumerator waiter()
    {
        //Wait for 4 seconds
        yield return new WaitForSecondsRealtime(waitTime);

    }

    private void resetItem(){
        isTracking = true;
        StopAllCoroutines();
        transform.position = m_StartPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        StartCoroutine(waiter());
        StartCoroutine(WaitThenDie());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Destroyable"){
            Debug.Log("destroy object");
            Destroy(other.gameObject);
            DisplayMessage(msg);
            resetItem();
        }

        if (other.gameObject.tag == "Cannon Receiver") {
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
