using Communication;
using UnityEngine;

namespace Player
{
    public class CommunicableBuilding : MonoBehaviour, ICommunicable
    {
        [field: SerializeField] public Transform CommunicationTransform { get; private set; }
        [field: SerializeField] public Transform CommunicationViewpointTransform { get; private set; }

        public void StartCommunication()
        {
            
        }

        public void StopCommunication()
        {

        }
    }
}