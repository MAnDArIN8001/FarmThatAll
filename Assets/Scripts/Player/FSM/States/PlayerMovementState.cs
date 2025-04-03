using Player.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using Utiles.FSM;

namespace Player.FSM.States
{
    public class PlayerMovementState : State
    {
        private readonly MovementSystem _movementSystem;

        private readonly BaseInput _input;

        private readonly Camera _mainCamera;
        
        public PlayerMovementState(StateType stateType, MovementSystem movementSystem, BaseInput input)
        {
            StateType = stateType;
            _movementSystem = movementSystem;
            _input = input;
            _mainCamera = Camera.main;
        }
        
        public override void Enter()
        {
            ComputeDestination();

            _input.Mouse.Click.performed += HandleClick;
        }

        public override void Update() { }

        public override void Exit()
        {
            _input.Mouse.Click.performed -= HandleClick;
        }

        private void HandleClick(InputAction.CallbackContext context)
        {
            ComputeDestination();
        }

        private void ComputeDestination()
        {
            var mousePosition = _input.Mouse.Position.ReadValue<Vector2>();

            var ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hitInfo))
            {
                _movementSystem.SetDestination(hitInfo.point);
            }
        }
    }
}