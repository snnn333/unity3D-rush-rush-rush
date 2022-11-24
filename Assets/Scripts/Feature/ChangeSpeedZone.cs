using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class ChangeSpeedZone : MonoBehaviour
    {
        [Tooltip("This is the speed for the player in this zone")]
        public float ZoneSpeed;

        // Effects
        public GameObject DashingEffect;
        public GameObject RunningEffect;
        private MovementCharacterController _movementController;
        private float _actualSpeed;
        private GameObject _player;

        private void SpawnEffectOnPlayer(GameObject myVFX)
    {
        if (myVFX != null && _player) {
            GameObject spawnedVFX = Instantiate(myVFX, _player.transform.position, _player.transform.rotation) as GameObject; 
            // Destroy(spawnedVFX, 5f);
        }
    }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _movementController = other.GetComponent<MovementCharacterController>();
                _player = other.gameObject;
                _movementController.ChangeSpeed(ZoneSpeed);
                _actualSpeed = ZoneSpeed;
                SpawnEffectOnPlayer(DashingEffect);
                // SpawnEffectOnPlayer(RunningEffect);
                // _movementController.DeactivatePlayerControlByTime(10);
                // StartCoroutine(WaitAndSlowingDown());
            }
        }

        private IEnumerator WaitAndSlowingDown() {
            yield return new WaitForSeconds(2);
            // _isSlowingDown = true;
            // Destroy(RunningEffect);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<MovementCharacterController>().ResetOriginalSpeed();
                // _isSlowingDown = false;
            }
        }

        // private void Update() {
        //     // Regress the dashing speed
        //     if (_isSlowingDown && _actualSpeed > _movementController._originalRunningSpeed) {
        //         _actualSpeed -= 0.1f;
        //         _movementController.ChangeSpeed(_actualSpeed);
        //     }

        //     if (_isSlowingDown && _actualSpeed <= _movementController._originalRunningSpeed) {
        //         _movementController.ResetOriginalSpeed();
        //         _isSlowingDown = false;
        //     }
            
        // }
    }
}