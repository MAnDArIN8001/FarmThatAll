using System.Collections.Generic;
using DG.Tweening;
using Player.Controls;
using Player.FSM;
using Player.FSM.States;
using Player.CameraControls;
using Player.Setups;
using UnityEngine;
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

        private BaseInput _baseInput;

        private PlayerStateMachine _playerStateMachine;

        [Inject]
        private void Initialize(BaseInput input)
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
                    () => _baseInput.Controls.StopAction.WasPerformedThisFrame()),
                
                new Transition(StateType.Movement, StateType.Idle, 
                    () => _movementSystem.IsMovementDone)
            };

            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, new PlayerIdleState(StateType.Idle) },
                { StateType.Movement, new PlayerMovementState(StateType.Movement, _movementSystem, _pointerSystem, _baseInput) },
                { StateType.Communication, new PlayerCommunicationState(StateType.Communication, _pointerSystem, _cameraSystem, transform, _rotationAnimationSetup) },
            };

            _playerStateMachine = new PlayerStateMachine(states, transitions);
        }

        private void Update()
        {
            _playerStateMachine?.Update();
        }

        private void LateUpdate()
        {
            _playerStateMachine?.LateUpdate();
        }
    }
}