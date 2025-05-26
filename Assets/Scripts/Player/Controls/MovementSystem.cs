using System;
using UnityEngine;
using UnityEngine.AI;
using Utiles.FSM;

namespace Player.Controls
{
    public class MovementSystem : MonoBehaviour
    {
        public bool IsMovementDone { get; private set; }

        [SerializeField] private NavMeshAgent _agent;

        private void LateUpdate()
        {
            if (IsMovementDone || _agent.pathPending)
            {
                return;
            }
            
            IsMovementDone = _agent.remainingDistance <= _agent.stoppingDistance;
        }

        public void SetDestination(Vector3 point)
        {
            _agent.isStopped = false;
            
            if (_agent is null)
            {
                return;
            }

            _agent.SetDestination(point);

            IsMovementDone = false;
        }

        public void BreakMovement()
        {
            IsMovementDone = true;
            _agent.isStopped = true;
        }
    }
}