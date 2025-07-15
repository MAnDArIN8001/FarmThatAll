using UnityEngine;

namespace Utiles
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        [SerializeField] private Vector3 _rotationDirection;

        private void Update()
        {
            transform.Rotate(_rotationDirection * Time.deltaTime * _speed);
        }
    }
}