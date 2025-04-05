using Communication;
using UnityEngine;

namespace Player.Controls
{
    public class PointerSystem : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private LayerMask _obstacleLayers;

        public Vector3 LastValidPoint { get; private set; }

        public ICommunicable PointedCommunicable { get; private set; }

        private void Awake()
        {
            _camera = Camera.main;
        }

        public bool CheckIsPointReachable(Vector2 screenPoint)
        {
            var ray = _camera.ScreenPointToRay(screenPoint);

            if (Physics.Raycast(ray, out var hitInfo) && hitInfo.collider is not null && !IsObjectInLayer(hitInfo.collider.gameObject))
            {
                Vector3 validPoint = hitInfo.point;

                if (hitInfo.collider.TryGetComponent<ICommunicable>(out var communicable))
                {
                    validPoint = communicable.CommunicationTransform.position;
                }
                
                PointedCommunicable = communicable;
                LastValidPoint = validPoint;

                return true;
            }
            
            return false;
        }

        private bool IsObjectInLayer(GameObject target) => (_obstacleLayers.value & (1 << target.layer)) != 0;
    }
}