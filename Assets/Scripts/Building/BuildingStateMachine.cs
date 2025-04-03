using System;
using System.Collections.Generic;
using Building.BuildingSystemStates;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utiles.FSM;
using State = Utiles.FSM.State;

namespace Building
{
    public class BuildingStateMachine : IDisposable
    {
        public LayerMask TerrainMask {get; private set;}
        private Material _shapeMaterial;
        
        private Building _currentBuilding;
        
        private State _currentState;
        
        private IDictionary<StateType, State> _states;
        private List<Transition> _transitions;

        public BuildingStateMachine(IDictionary<StateType, State> states,List<Transition> transitions)
        {
            _states = states;
            _transitions = transitions;
            
            ChangeState(StateType.Idle);
        }
        public void Update()
        {
            _currentState?.Update();

            for (int i = 0; i < _transitions.Count; i++)
            {
                var transition = _transitions[i];
                
                if (_currentState.StateType == transition.From && transition.Condition())
                {
                    ChangeState(transition.To);
                }
            }
        }

        private void ChangeState(StateType state)
        {
            if (!_states.TryGetValue(state, out State newState))
            {
                Debug.LogError($"Unknown building state: {state}");
                
                return;
            }
            
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Dispose()
        {
            _states = null;
            _transitions = null;
        }
    }
}