using Communication;
using Player.Controls;
using UnityEngine;
using Utiles.FSM;

namespace Player.FSM.States
{
    public class PlayerCommunicationState : State
    {
        private readonly PointerSystem _pointerSystem;

        private ICommunicable _communicable;
        
        public PlayerCommunicationState(StateType stateType, PointerSystem pointerSystem)
        {
            StateType = stateType;
            _pointerSystem = pointerSystem;
        }
        
        public override void Enter()
        {
            _communicable = _pointerSystem.PointedCommunicable;
            
            _communicable.Communicate();
            
            Debug.Log("Enter communication");
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}