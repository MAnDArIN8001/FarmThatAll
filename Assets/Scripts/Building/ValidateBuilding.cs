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
        public int terrainLayer;
        
        private void Start()
        {
            _colliders = new List<Collider>();
            terrainLayer = LayerMask.NameToLayer("Terrain");

            if (_colliders.Count == 0)
            {
                OnToggleValidity?.Invoke(true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == terrainLayer)
                return;
            
            AddCollider(other);
        }

        private void OnTriggerExit(Collider other)
        { 
            if (other.gameObject.layer == terrainLayer)
                return;
            
            RemoveCollider(other);
        }

        private void AddCollider(Collider collider)
        {
            if (_colliders.Count == 0)
                OnToggleValidity?.Invoke(false);
            _colliders.Add(collider);
        }

        private void RemoveCollider(Collider collider)
        {
            _colliders.Remove(collider);
            if (_colliders.Count == 0)
            {
                OnToggleValidity?.Invoke(true);
            }
        }
    }
}