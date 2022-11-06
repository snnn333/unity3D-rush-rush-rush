using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelPlayer : MonoBehaviour
{
    public GameObject Player = null;
    
    public float acceleration = 5f;
    public Vector3 m_StartPosition;
    public float follow_distance = 10f;
    // Stop tracking the player within this distance, so player can take action to dodge the bullet
    public float stop_follow_distance = 0f;
    public bool resetAfterDie = true;
    private Vector3 m_Direction;
    private Quaternion m_Rotation;
    // Whether the bullet is tracking the player
    private bool isTracking = true;
    private float speed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_StartPosition = transform.position;
        m_Rotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, m_Rotation, Time.time * 1000f);
        Player = GameObject.FindWithTag("Player");

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
            }else{
                isTracking = true;
            }
            
            if(isTracking && playerBulletDistance < follow_distance){
                Debug.Log("tracking");
                Debug.Log(playerBulletDistance);
                speed = acceleration * (follow_distance - playerBulletDistance + 0.1f) / (follow_distance+0.1f) ;
                var diff = transform.position - Player.transform.position;
                diff.y = 0;
                transform.rotation = Quaternion.LookRotation( diff);
                transform.Translate(transform.forward*speed*Time.deltaTime, Space.World);
            }else{
                if(!isTracking){
                    speed = 0;
                }else{
                    if(speed > 0){
                        speed -= 0.5f;
                        if(speed < 0){
                            speed = 0;
                        }
                        transform.Translate(transform.forward*speed*Time.deltaTime, Space.World);
                    }
                }
                
            }
            
        }
    }
}
