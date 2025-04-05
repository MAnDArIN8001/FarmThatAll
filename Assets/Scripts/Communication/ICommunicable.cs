using UnityEngine;

namespace Communication
{
    public interface ICommunicable
    {
        public Transform CommunicationTransform { get; }

        public void Communicate();
    }
}