using System.Collections.Generic;
using Building.BuildingSystemStates;
using UnityEngine;
using UnityEngine.Serialization;
using Utiles.EventSystem;
using Utiles.FSM;
using Zenject;

namespace Building
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingShapeMaterial;
        
        [SerializeField] private BuildingData buildingShape;
        
        private BuildingStateMachine _buildingStateMachine;
        private BuildingActiveState _buildingActiveState;
        
        private BaseInput _input;
        private EventBus _eventBus;
        
        [Inject]
        private void Initialize(BaseInput input, EventBus eventBus)
        {
            _input = input;
            _eventBus = eventBus;
            
            if (_buildingActiveState == null)
            {
                _buildingActiveState =
                    new BuildingActiveState(StateType.Active, _eventBus, buildingShape, buildingShapeMaterial, _input);
            }
            
            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, new BuildingIdleState(StateType.Idle) },
                { StateType.Active, _buildingActiveState}
            };

            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Active,
                    () => _input.Mouse.Click.WasPerformedThisFrame()),
                new Transition(StateType.Active, StateType.Idle,
                    () => _input.Mouse.RightClick.WasPerformedThisFrame())
            };
            
            _buildingStateMachine = new BuildingStateMachine(states, transitions);
        }

        private void Update()
        {
            _buildingStateMachine?.Update();
        }

        private bool BuildingCalled(BuildingData buildingData)
        {
            
            return true;
        }
    }
}