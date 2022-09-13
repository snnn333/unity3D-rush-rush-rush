using UnityEngine;

namespace PlatformCharacterController
{
    public class TopDownCamera : MonoBehaviour
    {
        public Transform Target;

        public float SeeForward;

        public float RotationSpeed = 3;

        // The speed with which the camera will be following.
        public float Smoothing = 5f;

        // The initial offset from the target.
        private Vector3 _offset;

        private bool _rotate;
        private bool _rotateToLeft;
        private bool _rotateToRight;
        private bool _mobile;

        private void Start()
        {
            _offset = transform.position - Target.position;
        }

        private void Update()
        {
            if (_mobile) return;
            _rotateToLeft = Input.GetKey(KeyCode.Q);
            _rotateToRight = Input.GetKey(KeyCode.E);
        }

        private void FixedUpdate()
        {
            if (_rotateToLeft)
            {
                TurnCameraToLeft();
            }

            if (_rotateToRight)
            {
                TurnCameraToRight();
            }

            var targetCamPos = Target.position + _offset + Target.forward * SeeForward;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, Smoothing * Time.deltaTime);
        }

        public void MobileToLeft(bool active)
        {
            _rotateToLeft = active;
            _mobile = true;
        }

        public void MobileToRight(bool active)
        {
            _rotateToRight = active;
            _mobile = true;
        }

        public void TurnCameraToLeft()
        {
            transform.Rotate(Vector3.up * -RotationSpeed, Space.World);
        }

        public void TurnCameraToRight()
        {
            transform.Rotate(Vector3.up * RotationSpeed, Space.World);
        }
    }
}