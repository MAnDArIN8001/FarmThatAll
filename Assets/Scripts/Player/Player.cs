using System;
using System.Collections.Generic;
using Player.Controls;
using Player.FSM;
using Player.FSM.States;
using UnityEngine;
using Utiles.FSM;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [Header("Systems")]
        [field: SerializeField, Space] public MovementSystem MovementSystem { get; private set; }

        private BaseInput _baseInput;

        private PlayerStateMachine _playerStateMachine;

        [Inject]
        private void Initialize(BaseInput input)
        {
            _baseInput = input;

            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Movement,() => _baseInput.Mouse.Click.WasPerformedThisFrame()),
                new Transition(StateType.Movement, StateType.Idle, () => MovementSystem.IsMovementDone)
            };

            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, new PlayerIdleState(StateType.Idle) },
                { StateType.Movement, new PlayerMovementState(StateType.Movement, MovementSystem, _baseInput) }
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