using System.Collections.Generic;
using Building.BuildingSystemStates;
using UnityEngine;
using Utiles.FSM;
using Zenject;

namespace Building
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingShapeMaterial;
        
        [SerializeField] private Building buildingToPlace;
        
        private BuildingStateMachine _buildingStateMachine;
        private BuildingActiveState _buildingActiveState;
        private BaseInput _input;

        [Inject]
        private void Initialize(BaseInput input)
        {
            _input = input;

            if (_buildingActiveState == null)
            {
                _buildingActiveState =
                    new BuildingActiveState(StateType.Active, buildingToPlace, buildingShapeMaterial, _input);
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
    }
}