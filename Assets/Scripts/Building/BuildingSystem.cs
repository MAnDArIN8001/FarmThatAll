using System;
using System.Collections.Generic;
using Building.BuildingSystemStates;
using UnityEngine;
using Utiles.EventSystem;
using Utiles.Factory;
using Utiles.FSM;
using Zenject;

namespace Building
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingShapeMaterial;
        
        private StateMachine _buildingStateMachine;
        private BuildingActiveState _buildingActiveState;
        
        private BaseInput _input;
        
        private EventBus _eventBus;
        
        [Inject]
        private void Initialize(BaseInput input, EventBus eventBus, MonoAbstractFactory factory)
        {
            _input = input;
            _eventBus = eventBus;
            
            if (_buildingActiveState == null)
            {
                _buildingActiveState =
                    new BuildingActiveState(StateType.Active, _eventBus, buildingShapeMaterial, _input, factory);
            }
            
            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, new BuildingIdleState(StateType.Idle) },
                { StateType.Active, _buildingActiveState}
            };

            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Active,
                    () => _eventBus.WasInvokedThisFrame<BuildingData>()),
                new Transition(StateType.Active, StateType.Idle,
                    () => _input.Mouse.RightClick.WasPerformedThisFrame())
            };
            
            _buildingStateMachine = new StateMachine(states, transitions, StateType.Idle);
        }

        private void Update()
        {
            _buildingStateMachine?.Update();
        }

        private void LateUpdate()
        {
            _buildingStateMachine?.LateUpdate();
        }
    }
}