using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullets : MonoBehaviour
{
    public GameObject Player = null;
    public GameObject bulletPrefab;
    public float waitTime = 10f;
    public Vector3 bulletPosition;
    public bool tracking = false;
    public float trackingDistance = 10f;
    // private Quaternion bulletRotation = null;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        loop();
        // if(bulletRotation == null){
        //     bulletRotation = transform.rotation;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if(tracking){
            trackPlayer();
        }
    }

    private void loop(){
        launchBullets();
        StartCoroutine(waitAndShoot());
    }

    private void trackPlayer(){
        var playerPosition = Player.transform.position;
        float playerBulletDistance = Vector3.Distance(transform.position,playerPosition);
        if(playerBulletDistance < trackingDistance){
            transform.LookAt(Player.transform); 
        }
    }

    private void launchBullets(){
        bulletPosition = transform.GetChild(1).transform.position;      
        var bullet = Instantiate(bulletPrefab, bulletPosition, transform.rotation);
        var bulletTrack = bullet.GetComponent<Bullet>();
        if(bulletTrack != null){
            bulletTrack.lifeTime = waitTime;
        }
    }

    IEnumerator waitAndShoot()
    {
        //Wait for 4 seconds
        yield return new WaitForSecondsRealtime(waitTime);
        loop();

    }
}
