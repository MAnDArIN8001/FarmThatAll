using UnityEngine;
using UnityEngine.AI;
using Utiles.FSM;

namespace Player.Controls
{
    public class MovementSystem : MonoBehaviour
    {
        public bool IsMovementDone { get; private set; }

        [SerializeField] private NavMeshAgent _agent;

        public void SetDestination(Vector3 point)
        {
            if (_agent is null)
            {
                Debug.LogError($"Agent for {this} system doesnt initialized");

                return;
            }

            _agent.SetDestination(point);
        }
    }
}