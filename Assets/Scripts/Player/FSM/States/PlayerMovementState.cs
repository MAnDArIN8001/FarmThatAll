using Player.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using Utiles.FSM;

namespace Player.FSM.States
{
    public class PlayerMovementState : State
    {
        private readonly MovementSystem _movementSystem;
        private readonly PointerSystem _pointerSystem;

        private readonly BaseInput _input;

        private readonly Camera _mainCamera;
        
        public PlayerMovementState(StateType stateType, MovementSystem movementSystem, PointerSystem pointerSystem, BaseInput input)
        {
            StateType = stateType;
            
            _movementSystem = movementSystem;
            _pointerSystem = pointerSystem;
            _input = input;
            _mainCamera = Camera.main;
        }
        
        public override void Enter()
        {
            ComputeDestination();

            _input.Mouse.LeftClick.performed += HandleClick;
        }

        public override void Update() { }

        public override void Exit()
        {
            _input.Mouse.LeftClick.performed -= HandleClick;
            
            _movementSystem.BreakMovement();
        }

        private void HandleClick(InputAction.CallbackContext context)
        {
            ComputeDestination();
        }

        private void ComputeDestination()
        {
            if (_pointerSystem.CheckIsPointReachable(_input.Mouse.Position.ReadValue<Vector2>()))
            {
                _movementSystem.SetDestination(_pointerSystem.LastValidPoint);
            }
        }
    }
}