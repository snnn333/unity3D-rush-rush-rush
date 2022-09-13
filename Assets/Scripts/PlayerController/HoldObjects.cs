using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class HoldObjects : MonoBehaviour
    {
        public float CarryMovementSpeed;
        public bool Busy;
        public bool BadPosition;
        public Transform CarryPosition;
        public GameObject CurrentItem;

        public MovementCharacterController MovementCharacterControllerComponent;

        private GameObject _closeItem;

        private bool _dropCarryItem;

        private void Awake()
        {
            if (!MovementCharacterControllerComponent)
            {
                MovementCharacterControllerComponent = GetComponent<MovementCharacterController>();
            }
        }

        private void Update()
        {
            _dropCarryItem = MovementCharacterControllerComponent.PlayerInputs.DropCarryItem();

            if (_dropCarryItem)
            {
                DropOrCarryItem();
            }
        }

        public void DropOrCarryItem()
        {
            if (Busy)
            {
                DropItem();
            }
            else if (_closeItem)
            {
                CarryItem();
            }
        }

        public void CarryItem()
        {
            if (Busy) return;
            if (BadPosition) return;
            Busy = true;
            CarryObject(_closeItem);
        }

        public void DropItem()
        {
            if (!Busy) return;
            Busy = false;
            StartCoroutine(DropCarryItem());
        }

        private void CarryObject(GameObject itemToCarry)
        {
            StartCoroutine(CarryItem(itemToCarry));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<LoadableObject>()) return;

            if (other.GetComponent<LoadableObject>().InUse) return;

            _closeItem = other.gameObject;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_closeItem) return;
            if (!other.GetComponent<LoadableObject>()) return;

            if (other.GetComponent<LoadableObject>().InUse) return;

            _closeItem = other.gameObject;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<LoadableObject>()) return;

            if (other.GetComponent<LoadableObject>().InUse) return;

            _closeItem = null;
            BadPosition = false;
        }

        #region Animator

        //use this to set carry animations
        private void SetCarryAnimation(bool carry)
        {
            MovementCharacterControllerComponent.PlayerAnimator.SetBool("Carrying", carry);
        }

        #endregion

        #region Coroutine

        private IEnumerator CarryItem(GameObject itemToCarry)
        {
            MovementCharacterControllerComponent.LookAtTarget(itemToCarry.transform);
            MovementCharacterControllerComponent.ChangeSpeed(CarryMovementSpeed);

            MovementCharacterControllerComponent.HoldingObject = true;
            MovementCharacterControllerComponent.CanControl = false;
            itemToCarry.GetComponent<LoadableObject>().Catch();

            //Waiting time to use the player again, very useful to wait for the animation to collect the object to complete
            yield return new WaitForSeconds(0.1f);

            SetCarryAnimation(true);
            itemToCarry.GetComponent<LoadableObject>().ResetPosition(CarryPosition);

            CurrentItem = itemToCarry;
            MovementCharacterControllerComponent.CanControl = true;
            itemToCarry.transform.SetParent(CarryPosition);
        }

        private IEnumerator DropCarryItem()
        {
            SetCarryAnimation(false);
            MovementCharacterControllerComponent.CanControl = false;
            MovementCharacterControllerComponent.HoldingObject = false;

            CurrentItem.transform.SetParent(null);

            CurrentItem.GetComponent<LoadableObject>().Drop();
            //Waiting time to use the player again, very useful to wait for the animation to drop the object to complete
            yield return new WaitForSeconds(0.3f);

            MovementCharacterControllerComponent.CanControl = true;

            MovementCharacterControllerComponent.ResetOriginalSpeed();
        }

        #endregion
    }
}