using System;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private GameObject mapCube, mapCamera;
    private float speed;
    public bool canMove;
    public bool canSee;

    private void Start()
    {
        //do
        //{
        //    StartCoroutine(Wait(5.0f));
        //} while (!GameObject.Find("Player(Clone)"));
        mapCube = GameObject.Find("MapCamera/PlayerCube");
        mapCamera = GameObject.Find("MapCamera");
        
        speed = 1.0f;
        canMove = true;
        canSee = true;
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }
        //mapCube.transform.position = new Vector3(m_Character.transform.position.x, 24, m_Character.transform.position.z);
        //mapCamera.transform.position = new Vector3(m_Character.transform.position.x, 28, m_Character.transform.position.z);
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 1.2f;//1.2f
        }
        else
        {
            speed = 1.0f;//1.0f
        }
#endif

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump, speed);
        m_Jump = false;
    }

    //IEnumerator Wait(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    //等待之后执行的动作
    //}

    

    
}

