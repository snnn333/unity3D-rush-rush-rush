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
    private bool isMoving = true;
    public float follow_distance = 5f;
    public float lifeTime = 10f;
    public float waitTime = 2f;
    private Vector3 m_Direction;
    private Quaternion m_Rotation;
    // public Enums.Directions useSide = Enums.Directions.Up;
    // Start is called before the first frame update
    void Start()
    {
        m_StartPosition = transform.position;
        m_Rotation = transform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        Player = GameObject.FindWithTag("Player");
        Debug.Log("player found");
        StartCoroutine(waiter());
        StartCoroutine(WaitThenDie());
    }

    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(lifeTime);
        transform.position = m_StartPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        StartCoroutine(waiter());
        StartCoroutine(WaitThenDie());
        
    }



    // Update is called once per frame
    void Update()
    {
        if(Player != null){
            var playerPosition = Player.transform.position;
            if(Vector3.Distance(transform.position,playerPosition) < follow_distance){
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
                transform.LookAt(Player.transform); 
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destroyable")){
            Destroy(other.gameObject);
        }
        transform.position = m_StartPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        StartCoroutine(waiter());
        StartCoroutine(WaitThenDie());
        
    }
}
