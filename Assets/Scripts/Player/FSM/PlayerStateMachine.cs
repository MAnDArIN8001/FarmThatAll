using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utiles.FSM;

namespace Player.FSM
{
    public class PlayerStateMachine : IDisposable
    {
        private Dictionary<StateType, State> _states;

        private List<Transition> _transitions;

        private State _currentState;

        public PlayerStateMachine(Dictionary<StateType, State> states, List<Transition> transitions)
        {
            _states = states;
            _transitions = transitions;

            SetState(StateType.Idle);
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void LateUpdate()
        {
            for (int i = 0; i < _transitions.Count; i++)
            {
                var transition = _transitions[i];

                if (_currentState.StateType == transition.From && transition.Condition())
                {
                    SetState(transition.To);
                }
            }
        }

        private void SetState(StateType stateType)
        {
            if (!_states.TryGetValue(stateType, out var state))
            {
                Debug.LogError($"The state machine {this} doesnt contains state {stateType}");

                return;
            }
            
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        private void AddTransition(Transition newTransition)
        {
            if (_transitions.Contains(newTransition))
            {
                Debug.LogError($"The state machine {this} already contains transition {newTransition}");

                return;
            }
            
            _transitions.Add(newTransition);
        }

        private void AddState(StateType stateType, State state)
        {
            if (_states.ContainsKey(stateType))
            {
                Debug.LogError($"The state machine {this} already contains state type {stateType}");

                return;
            }
            
            if (_states.ContainsValue(state))
            {
                Debug.LogError($"The state machine {this} already contains state {state}");

                return;
            }
            
            _states.Add(stateType, state);
        }
        
        public void Dispose()
        {
            _states.Clear();
            _transitions.Clear();
        }
    }
}