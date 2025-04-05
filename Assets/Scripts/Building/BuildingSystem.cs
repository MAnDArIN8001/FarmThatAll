using System.Collections.Generic;
using Building.BuildingSystemStates;
using UnityEngine;
using Utiles.EventSystem;
using Utiles.FSM;
using Zenject;

namespace Building
{
    public class BuildingSystem : MonoBehaviour
    {
        private bool _isBuildingSelected;
        
        [SerializeField] private Material buildingShapeMaterial;
        
        private BuildingStateMachine _buildingStateMachine;
        private BuildingActiveState _buildingActiveState;
        
        private BaseInput _input;
        
        private EventBus _eventBus;
        
        [Inject]
        private void Initialize(BaseInput input, EventBus eventBus)
        {
            _input = input;
            _eventBus = eventBus;
            
            _eventBus.Subscribe<BuildingData>(BuildingSelectedHandler);
            
            if (_buildingActiveState == null)
            {
                _buildingActiveState =
                    new BuildingActiveState(StateType.Active, _eventBus, buildingShapeMaterial, _input);
            }
            
            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, new BuildingIdleState(StateType.Idle) },
                { StateType.Active, _buildingActiveState}
            };

            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Active,
                    () => _isBuildingSelected),
                new Transition(StateType.Active, StateType.Idle,
                    () => _input.Mouse.RightClick.WasPerformedThisFrame())
            };
            
            _buildingStateMachine = new BuildingStateMachine(states, transitions);
        }

        private void Update()
        {
            _buildingStateMachine?.Update();

            if (_isBuildingSelected)
            {
                _isBuildingSelected = false;
            }
        }

        private void BuildingSelectedHandler(BuildingData buildingData)
        {
            _isBuildingSelected = true;
        }
    }
}