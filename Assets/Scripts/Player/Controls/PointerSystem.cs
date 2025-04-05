using System;
using UnityEngine;

namespace Player.Controls
{
    public class PointerSystem : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private LayerMask _obstacleLayers;

        public Vector3 LastValidPoint { get; private set; }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private bool CheckIsPointReachable(Vector2 screenPoint)
        {
            var ray = _camera.ScreenPointToRay(screenPoint);

            if (Physics.Raycast(ray, out var hitInfo) && hitInfo.collider is not null && !IsObjectInLayer(hitInfo.collider.gameObject))
            {
                LastValidPoint = hitInfo.collider.transform.position;

                return true;
            }
            
            return false;
        }

        private bool IsObjectInLayer(GameObject target) => (_obstacleLayers.value & (1 << target.layer)) != 0;
    }
}