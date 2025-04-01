using System;
using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    [RequireComponent(typeof(Collider))]
    public class ValidateBuilding : MonoBehaviour
    {
        public event Action<bool> OnToggleValidity;
        private List<Collider> _colliders;
        
        private void Start()
        {
            _colliders = new List<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                return;
            
            AddCollider(other);
        }

        private void OnTriggerExit(Collider other)
        { 
            if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                return;
            
            RemoveCollider(other);
        }

        private void AddCollider(Collider collider)
        {
            if (_colliders.Count == 0)
                OnToggleValidity?.Invoke(false);
            _colliders.Add(collider);
            Debug.Log($"Adding collider {_colliders.Count}");
        }

        private void RemoveCollider(Collider collider)
        {
            _colliders.Remove(collider);
            Debug.Log($"Removing collider {_colliders.Count}");
            if (_colliders.Count == 0)
            {
                OnToggleValidity?.Invoke(true);
            }
        }
    }
}