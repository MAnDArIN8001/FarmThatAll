using UnityEngine;

namespace Communication
{
    public interface ICommunicable
    {
        public Transform CommunicationTransform { get; }
        public Transform CommunicationViewpointTransform { get; }

        public void Communicate();
    }
}