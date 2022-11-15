using System;
using System.Collections;
using UnityEngine;

namespace PlatformCharacterController
{
    public class MovementCharacterController : MonoBehaviour
    {
        [Header("Player Controller Settings")] [Tooltip("Speed for the player.")]
        public float RunningSpeed = 5f;

        [Tooltip("Slope angle limit to slide.")]
        public float SlopeLimit = 45;

        [Tooltip("Slide friction.")] [Range(0.1f, 0.9f)]
        public float SlideFriction = 0.3f;

        [Tooltip("Gravity force.")] [Range(0, -100)]
        public float Gravity = -30f;

        [Tooltip("Max speed for the player when fall.")] [Range(0, 100)]
        public float MaxDownYVelocity = 15;

        [Tooltip("Can the user control the player?")]
        public bool CanControl = true;

        [Header("Jump Settings")] [Tooltip("This allow the character to jump.")]
        public bool CanJump = true;

        [Tooltip("Jump max elevation for the character.")]
        public float JumpHeight = 2f;

        [Tooltip("This allow the character to jump in air after another jump.")]
        public bool CanDoubleJump = true;

        [Header("Dash Settings")] [Tooltip("The player have dash?.")]
        public bool CanDash = true;

        [Tooltip("Cooldown for the dash.")] public float DashCooldown = 3;

        [Tooltip("Force for the dash, a greater value more distance for the dash.")]
        public float DashForce = 5f;

        [Header("JetPack")] [Tooltip("Player have jetpack?")]
        public bool Jetpack = true;

        [Tooltip("The fuel maxima capacity for the jetpack.")]
        public float JetPackMaxFuelCapacity = 90;

        [Tooltip("The current fuel for the jetpack, if 0 the jet pack off.")]
        public float JetPackFuel;

        [Tooltip("The force for the jetpack, this impulse the player up.")]
        public float JetPackForce;

        [Tooltip("Jet pack consume this quantity by second active.")]
        public float FuelConsumeSpeed;

        [Header("SlowFall")] [Tooltip("This allow the player a slow fall, you can use an item like a parachute.")]
        public bool HaveSlowFall;

        [Tooltip("Speed vertical for the slow fall.")] [Range(0, 5)]
        public float SlowFallSpeed = 1.5f;

        [Tooltip("Slow fall forward speed.")] [Range(0, 1)]
        public float SlowFallForwardSpeed = 0.1f;

        [Header("Push settings:")]
        [Tooltip(
            "True to only move the object in the opposite direction to the pushed face. False to move the object based on where it is pushed.")]
        public bool PushInFixedDirections;

        [Tooltip("Force of pushing objects")] public float PushPower = 2.0f;

        [Tooltip("This is the drag force for the character, a standard value are (8, 0, 8). ")]
        public Vector3 DragForce;

        [Tooltip("Player Status: Holds or not an object")]
        public bool HoldingObject;
        [Tooltip("Player Status: Swimming")]
        public bool Swimming;

        [Tooltip("This is the animator for you character.")]
        public Animator PlayerAnimator;

    
        [Header("Effects")] [Tooltip("This position is in the character feet and is use to instantiate effects.")]
        public Transform LowZonePosition;

        public GameObject JumpEffect;
        public GameObject DashEffect;
        public GameObject JetPackObject;
        public GameObject SlowFallObject;

        [Header("Use this to capture inputs")] public Inputs PlayerInputs;

        [Header("Platforms")] public Transform CurrentActivePlatform;

        private Vector3 _moveDirection;
        private Vector3 _activeGlobalPlatformPoint;
        private Vector3 _activeLocalPlatformPoint;
        private Quaternion _activeGlobalPlatformRotation;
        private Quaternion _activeLocalPlatformRotation;
        private Transform _characterTransform;

        //private vars
        private CharacterController _controller;
        private Vector3 _velocity;

        //Input.
        private float _horizontal;
        private float _vertical;

        private bool _jump;
        private bool _dash;
        private bool _flyJetPack;
        private bool _slowFall;
        public Health Health;
        //get direction for the camera
        private Transform _cameraTransform;
        private Vector3 _forward;
        private Vector3 _right;

        //temporal vars
        private float _originalRunningSpeed;
        private float _dashCooldown;
        private float _gravity;
        private bool _doubleJump;
        private bool _invertedControl;
        private bool _isCorrectGrounded;
        private bool _isGrounded;
        private bool _activeFall;
        private Vector3 _hitNormal;
        private Vector3 _move;
        private Rigidbody rigid;
        private Vector3 _direction;
        [SerializeField] float fallThreshold = 5f;
        public float previousY = 0f;

        public Health health;

        public GameObject gamePassObj;

        public GameObject checkPointObj;
        
        private void Awake()
        {
            PlayerInputs = GetComponent<Inputs>();
            _controller = GetComponent<CharacterController>();
            rigid = GetComponent<Rigidbody>();
            _characterTransform = transform;
            _originalRunningSpeed = RunningSpeed;
            health = GetComponent<Health>();
        }

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            _dashCooldown = DashCooldown;
            _gravity = Gravity;
        }

        private void Update()
        {
            if (health.currentHealth <= 0 || gamePassObj != null && gamePassObj.activeInHierarchy)
            {
                return;
            }
            
            bool previous = _isGrounded;
            CheckGroundStatus();
            if (previous && !_isGrounded )
            {   
                // Debug.Log("Falling");
                previousY = _characterTransform.position.y;
                Debug.Log("Previous Y: " + previousY);
            }
            
            if (!previous && _isGrounded)
            {   
                if (_velocity.y * -1 > fallThreshold)
                {
                    // check if the player is falling a certain distance
                    // if so, do damage
                    // if (previousY - _characterTransform.position.y > 5f)
                    // {
                    //     Debug.Log("Fell from a great height");
                    //     int damage = 1;
                    //     GameObject.Find("Player").GetComponent<Health>().TakeDamage("Fall", damage);    
                    // }
                    
                    
                    // int damage = 1;
                    // GameObject.Find("Player").GetComponent<Health>().TakeDamage(damage);
                    // Debug.Log("Do Damage" + _velocity.y);
                }
                // Debug.Log("Do Damage" + (_velocity.y * -1));
            }
            //capture input in this region, you can use PlayerInput class or simple replace "_jump = PlayerInputs.Jump()" whit  _jump = Input.GetButtonDown("buttonName") for example.
            _horizontal = PlayerInputs.GetHorizontal();
            _vertical = PlayerInputs.GetVertical();
            _jump = PlayerInputs.Jump();
            _dash = PlayerInputs.Dash();
            _flyJetPack = PlayerInputs.JetPack();
            _activeFall = PlayerInputs.Parachute();

            //this invert controls 
            if (_invertedControl)
            {
                _horizontal *= -1;
                _vertical *= -1;
                _jump = PlayerInputs.Dash();
                _dash = PlayerInputs.Jump();
            }

            if (_jump && !HoldingObject)
            {
                Jump(JumpHeight);
            }

            if (_dash && !HoldingObject)
            {
                Dash();
            }

            //if player can control the character
            if (CanControl)
            {
                //jet pack
                if (Jetpack && _flyJetPack && JetPackFuel > 0 && !HoldingObject)
                {
                    //if slow fall is active deactivate.
                    if (_slowFall)
                    {
                        _slowFall = false;
                        SlowFallObject.SetActive(false);
                    }

                    FlyByJetPack();
                }

                //slow fall
                if (_activeFall)
                {
                    _slowFall = !_slowFall;
                    _activeFall = false;
                }
            }
            else
            {
                _horizontal = 0;
                _vertical = 0;
            }

            //dash cooldown
            if (DashCooldown > 0)
            {
                DashCooldown -= Time.fixedDeltaTime;
            }
            else
            {
                DashCooldown = 0;
            }
            //set running animation

            SetRunningAnimation((Math.Abs(_horizontal) > 0 || Math.Abs(_vertical) > 0));

            //platforms
            if (!CurrentActivePlatform || !CurrentActivePlatform.CompareTag("Platform")) return;
            if (CurrentActivePlatform)
            {
                var newGlobalPlatformPoint = CurrentActivePlatform.TransformPoint(_activeLocalPlatformPoint);
                _moveDirection = newGlobalPlatformPoint - _activeGlobalPlatformPoint;
                if (_moveDirection.magnitude > 0.01f)
                {
                    _controller.Move(_moveDirection);
                }

                if (!CurrentActivePlatform) return;

                // Support moving platform rotation
                var newGlobalPlatformRotation = CurrentActivePlatform.rotation * _activeLocalPlatformRotation;
                var rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(_activeGlobalPlatformRotation);
                // Prevent rotation of the local up vector
                rotationDiff = Quaternion.FromToRotation(rotationDiff * Vector3.up, Vector3.up) * rotationDiff;
                _characterTransform.rotation = rotationDiff * _characterTransform.rotation;
                _characterTransform.eulerAngles = new Vector3(0, _characterTransform.eulerAngles.y, 0);

                UpdateMovingPlatform();
            }
            else
            {
                if (!(_moveDirection.magnitude > 0.01f)) return;
                _moveDirection = Vector3.Lerp(_moveDirection, Vector3.zero, Time.deltaTime);
                _controller.Move(_moveDirection);
            }
        }

        private void FixedUpdate()
        {
            if (CanControl)
            {
                //this activate or deactivate jet pack Object and effect.
                JetPackObject.SetActive(Jetpack && _flyJetPack && JetPackFuel > 0 && !HoldingObject);

                if (HaveSlowFall && !_isGrounded && _slowFall)
                {
                    SlowFall();
                }
                else
                {
                    SlowFallObject.SetActive(false);
                    _slowFall = false;
                }
            }

            //get the input direction for the camera position.
            _forward = _cameraTransform.TransformDirection(Vector3.forward);
            _forward.y = 0f;
            _forward = _forward.normalized;
            _right = new Vector3(_forward.z, 0.0f, -_forward.x);

            _move = (_horizontal * _right + _vertical * _forward);
            _direction = (_horizontal * _right + _vertical * _forward);


            //if no is correct grounded then slide.
            if (!_isCorrectGrounded && _isGrounded)
            {
                _move.x += (1f - _hitNormal.y) * _hitNormal.x * (1f - SlideFriction);
                _move.z += (1f - _hitNormal.y) * _hitNormal.z * (1f - SlideFriction);
            }

            _move.Normalize();
            //move the player if no is active the slow fall(this avoid change the speed for the fall)
            if (!_slowFall && _controller.enabled)
            {
                _controller.Move(Time.deltaTime * RunningSpeed * _move);
            }

            //Check if is correct grounded.
            _isCorrectGrounded = (Vector3.Angle(Vector3.up, _hitNormal) <= SlopeLimit);

            //set the forward direction
            if (_direction != Vector3.zero)
            {
                transform.forward = _direction;
            }

            //gravity force
            if (_velocity.y >= -MaxDownYVelocity)
            {
                _velocity.y += Gravity * Time.deltaTime;
            }
            
            //stop gravity if player are on water
            if (Swimming)
            {
                _velocity.y = 0;
            }

            _velocity.x /= 1 + DragForce.x * Time.deltaTime;
            _velocity.y /= 1 + DragForce.y * Time.deltaTime;
            _velocity.z /= 1 + DragForce.z * Time.deltaTime;
            if (_controller.enabled)
            {
                _controller.Move(_velocity * Time.deltaTime);
            }

            SetGroundedState();
        }

        public void Jump(float jumpHeight)
        {
            if (!CanJump || !CanControl)
            {
                return;
            }

            CurrentActivePlatform = null;
            //removing parachute if active;
            _slowFall = false;
            SlowFallObject.SetActive(false);

            //
            if (_isGrounded)
            {
                _hitNormal = Vector3.zero;
                SetJumpAnimation();
                _doubleJump = true;
                _velocity.y = 0;
                _velocity.y += Mathf.Sqrt(jumpHeight * -2f * Gravity);

                //Instantiate jump effect
                if (JumpEffect)
                {
                    Instantiate(JumpEffect, LowZonePosition.position, LowZonePosition.rotation);
                }
            }
            else if (CanDoubleJump && _doubleJump)
            {
                _doubleJump = false;
                _velocity.y = 0;
                _velocity.y += Mathf.Sqrt(jumpHeight * -2f * Gravity);

                //Instantiate jump effect
                if (JumpEffect)
                {
                    Instantiate(JumpEffect, LowZonePosition.position, LowZonePosition.rotation);
                }
            }
        }

        public void Dash()
        {
            if (!CanDash || DashCooldown > 0 || _flyJetPack)
            {
                return;
            }

            DashCooldown = _dashCooldown;

            if (DashEffect)
            {
                Instantiate(DashEffect, transform.position, _characterTransform.rotation);
            }

            SetDashAnimation();
            StartCoroutine(Dashing(DashForce / 10));
            _velocity += Vector3.Scale(transform.forward,
                DashForce * new Vector3((Mathf.Log(1f / (Time.deltaTime * DragForce.x + 1)) / -Time.deltaTime),
                    0, (Mathf.Log(1f / (Time.deltaTime * DragForce.z + 1)) / -Time.deltaTime)));
        }

        private void FlyByJetPack()
        {
            JetPackFuel -= Time.deltaTime * FuelConsumeSpeed;
            _velocity.y = 0;
            _velocity.y += Mathf.Sqrt(JetPackForce * -2f * Gravity);
        }

        //this is for a slow fall like a parachute.
        private void SlowFall()
        {
            SlowFallObject.SetActive(true);

            _controller.Move(transform.forward * SlowFallForwardSpeed);
            _velocity.y = 0;
            _velocity.y += -SlowFallSpeed;
        }

        //add fuel to the jet pack
        public void AddFuel(float fuel)
        {
            JetPackFuel += fuel;
            if (JetPackFuel > JetPackMaxFuelCapacity)
            {
                JetPackFuel = JetPackMaxFuelCapacity;
            }

            Debug.Log("Fuel +" + fuel);
        }

        public void ResetOriginalSpeed()
        {
            var holdComponent = GetComponent<HoldObjects>();
            if (holdComponent && holdComponent.Busy)
            {
                RunningSpeed = holdComponent.CarryMovementSpeed;
            }
            else
            {
                RunningSpeed = _originalRunningSpeed;
            }
        }

        public void LookAtTarget(Transform target)
        {
            transform.LookAt(target, Vector3.up);
            transform.rotation = Quaternion.Euler(0, _characterTransform.eulerAngles.y, 0);
        }

        //change the speed for the player
        public void ChangeSpeed(float speed)
        {
            RunningSpeed = speed;
        }

        //change the speed for the player for a time period
        public void ChangeSpeedInTime(float speedPlus, float time)
        {
            StartCoroutine(ModifySpeedByTime(speedPlus, time));
        }

        //invert player control(like a confuse skill)
        public void InvertPlayerControls(float invertTime)
        {
            //check if not are already inverted
            if (!_invertedControl)
            {
                StartCoroutine(InvertControls(invertTime));
            }
        }

        public void ActivateDeactivateJump(bool canJump)
        {
            CanJump = canJump;
        }

        public void ActivateDeactivateDoubleJump(bool canDoubleJump)
        {
            //if double jump is active activate normal jump
            if (canDoubleJump)
            {
                CanJump = true;
            }

            CanDoubleJump = canDoubleJump;
        }

        public void ActivateDeactivateDash(bool canDash)
        {
            CanDash = canDash;
        }

        public void ActivateDeactivateSlowFall(bool canSlowFall)
        {
            HaveSlowFall = canSlowFall;
        }

        public void ActivateDeactivateJetpack(bool haveJetPack)
        {
            Jetpack = haveJetPack;
        }

        private void UpdateMovingPlatform()
        {
            _activeGlobalPlatformPoint = transform.position;
            _activeLocalPlatformPoint = CurrentActivePlatform.InverseTransformPoint(_characterTransform.position);
            // Support moving platform rotation
            _activeGlobalPlatformRotation = transform.rotation;
            _activeLocalPlatformRotation =
                Quaternion.Inverse(CurrentActivePlatform.rotation) * _characterTransform.rotation;
        }

        private void CheckGroundStatus()
        {
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view

            Debug.DrawLine(
                transform.position + (Vector3.up * 0.1f),
                transform.position + Vector3.down * (_controller.height / 2 + 0.2f),
                Color.red
            );

#endif
            // it is also good to note that the transform position in the sample character is at the center
            var grounded = Physics.Raycast(transform.position, Vector3.down, _controller.height / 2 + 0.2f);

            _isGrounded = grounded || _controller.isGrounded;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //check for ground position status
            _hitNormal = hit.normal;

            // Make sure we are really standing on a straight platform
            // Not on the underside of one and not falling down from it either!
            if (hit.moveDirection.y < -0.9 && hit.normal.y > 0.41)
            {
                if (CurrentActivePlatform == hit.collider.transform) return;
                CurrentActivePlatform = hit.collider.transform;
                UpdateMovingPlatform();
            }
            else
            {
                CurrentActivePlatform = null;
            }

            var body = hit.collider.attachedRigidbody;

            //check for rigidbody
            if (body == null || body.isKinematic)
            {
                return;
            }

            //Use this to avoid moving objects below us
            if (hit.moveDirection.y < -0.3)
            {
                GetComponent<HoldObjects>().BadPosition = true;
                return;
            }

            GetComponent<HoldObjects>().BadPosition = false;

            // Calculate push direction from move direction,
            // Only push objects to the sides never up and down
            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // Apply the push

            if (PushInFixedDirections)
            {
                body.velocity = pushDir * PushPower;
            }
            else
            {
                body.AddForceAtPosition(pushDir * (PushPower * 10), hit.point);
            }
        }

        #region Animator

        private void SetRunningAnimation(bool run)
        {
            PlayerAnimator.SetBool("Running", run);
        }

        private void SetJumpAnimation()
        {
            PlayerAnimator.SetTrigger("Jump");
        }

        private void SetDashAnimation()
        {
            PlayerAnimator.SetTrigger("Dash");
        }

        private void SetGroundedState()
        {
            //avoid set the grounded var in animator multiple time
            if (PlayerAnimator.GetBool("Grounded") != _isGrounded)
            {
                PlayerAnimator.SetBool("Grounded", _isGrounded);
            }
        }

        #endregion

        #region Coroutine

        //Use this to deactivate te player control for a period of time.
        public IEnumerator DeactivatePlayerControlByTime(float time)
        {
            _controller.enabled = false;
            CanControl = false;
            yield return new WaitForSeconds(time);
            CanControl = true;
            _controller.enabled = true;
        }

        //dash coroutine.
        private IEnumerator Dashing(float time)
        {
            CanControl = false;
            if (!_isGrounded)
            {
                Gravity = 0;
                _velocity.y = 0;
            }

            //animate hear to true
            yield return new WaitForSeconds(time);
            CanControl = true;
            //animate hear to false
            Gravity = _gravity;
        }

        //modify speed by time coroutine.
        private IEnumerator ModifySpeedByTime(float speedPlus, float time)
        {
            if (RunningSpeed + speedPlus > 0)
            {
                RunningSpeed += speedPlus;
            }
            else
            {
                RunningSpeed = 0;
            }

            yield return new WaitForSeconds(time);
            RunningSpeed = _originalRunningSpeed;
        }

        private IEnumerator InvertControls(float invertTime)
        {
            yield return new WaitForSeconds(0.1f);
            _invertedControl = true;
            yield return new WaitForSeconds(invertTime);
            _invertedControl = false;
        }

        #endregion
    }
}