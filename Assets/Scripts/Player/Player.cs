using System.Collections.Generic;
using Building;
using Player.Controls;
using Player.FSM.States;
using Player.CameraControls;
using Player.Setups;
using UnityEngine;
using Utiles.EventSystem;
using Utiles.FSM;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [Header("Animation Settings")] 
        [SerializeField] private AnimationSetup _rotationAnimationSetup;
        
        [Header("Systems")]
        [SerializeField] private MovementSystem _movementSystem;
        [SerializeField] private PointerSystem _pointerSystem;
        [SerializeField] private CameraSystem _cameraSystem;

        [Space, SerializeField] private ControllableAnimator _animator;

        private BaseInput _baseInput;

        private StateMachine _stateMachine;

        [Inject]
        private void Initialize(BaseInput input, EventBus eventBus)
        {
            _baseInput = input;

            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Movement,
                    () => _baseInput.Mouse.LeftClick.WasPerformedThisFrame() 
                          && _pointerSystem.CheckIsPointReachable(_baseInput.Mouse.Position.ReadValue<Vector2>())),
                
                new Transition(StateType.Movement, StateType.Communication, 
                    () => _movementSystem.IsMovementDone 
                          && _pointerSystem.PointedCommunicable is not null),
                
                new Transition(StateType.Communication, StateType.Idle, 
                    () => _baseInput.Controls.StopAction.WasPerformedThisFrame() 
                || eventBus.WasInvokedThisFrame<BuildingData>()),
                
                new Transition(StateType.Movement, StateType.Idle, 
                    () => _movementSystem.IsMovementDone)
            };

            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, new PlayerIdleState(StateType.Idle) },
                { StateType.Movement, new PlayerMovementState(StateType.Movement, _movementSystem, _pointerSystem, _baseInput, _animator) },
                { StateType.Communication, new PlayerCommunicationState(StateType.Communication, _pointerSystem, _cameraSystem, transform, _rotationAnimationSetup, eventBus, _animator) },
            };

            _stateMachine = new StateMachine(states, transitions, StateType.Idle);
        }
        
        private void Update()
        {
            _stateMachine?.Update();
        }

        private void LateUpdate()
        {
            _stateMachine?.LateUpdate();
        }
    }
}