using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformCharacterController
{
    public class LoadableObject : MonoBehaviour
    {
        [Header("Item properties:")] public bool InUse;
        public Rigidbody RigidBodyComponent;

        public GameObject PopUpText;

        private GameObject _popUpText;
        private bool _itemUseGravity;
        private bool _itemIsKinematic;

        public void Drop()
        {
            //Returning these properties from the object's rigidbody
            if (RigidBodyComponent)
            {
                RigidBodyComponent.useGravity = _itemUseGravity;
                RigidBodyComponent.isKinematic = _itemIsKinematic;
            }

            ActivateCollider(true);
            InUse = false;
        }

        public void Catch()
        {
            InUse = true;

            DestroyPopUp();

            //Getting properties of the object's rigidbody
            if (!RigidBodyComponent) return;
            _itemUseGravity = RigidBodyComponent.useGravity;
            _itemIsKinematic = RigidBodyComponent.isKinematic;

            RigidBodyComponent.useGravity = false;
            RigidBodyComponent.isKinematic = true;
            ActivateCollider(false);
        }

        //reset item position when holding
        public void ResetPosition(Transform carryTransform)
        {
            transform.SetPositionAndRotation(carryTransform.position, carryTransform.rotation);
        }

        //Activate or deactivate all collider
        private void ActivateCollider(bool active)
        {
            foreach (var colliderInObject in gameObject.GetComponents<Collider>())
            {
                colliderInObject.enabled = active;
            }
        }

        private void DestroyPopUp()
        {
            if (!_popUpText) return;
            Destroy(_popUpText);
            _popUpText = null;
        }

        IEnumerator OnTriggerEnter(Collider other)
        {
            DestroyPopUp();
            yield return new WaitForSeconds(0.1f);
            //Use this to alert the player that he can collect the item if you need to.
            if (InUse || !other.CompareTag("Player")) yield break;
            if (other.GetComponent<HoldObjects>().BadPosition || other.GetComponent<HoldObjects>().Busy)
            {
                yield break;
            }

            if (!PopUpText) yield break;

            _popUpText = Instantiate(PopUpText, other.transform.position, transform.rotation,
                other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            //Use this to hide the alert
            if (InUse || !other.CompareTag("Player")) return;

            DestroyPopUp();
        }
    }
}