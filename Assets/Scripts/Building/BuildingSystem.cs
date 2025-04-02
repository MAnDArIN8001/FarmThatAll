using System.Collections.Generic;
using Building.BuildingSystemStates;
using UnityEngine;
using Utiles.FSM;
using VContainer;

namespace Building
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private Material buildingShapeMaterial;
        
        private BuildingStateMachine _buildingStateMachine;
        private BaseInput _input;

        [Inject]
        private void Initialize(BaseInput input)
        {
            _input = input;

            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, new BuildingIdleState(StateType.Idle) },
                { StateType.Active, new BuildingActiveState(StateType.Active, buildingShapeMaterial, _input)}
            };

            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Active, () => _input.Mouse.Click.WasPerformedThisFrame()),
                new Transition(StateType.Active, StateType.Idle, () => _input.Mouse.RightClick.WasPerformedThisFrame())
            };
            
            _buildingStateMachine = new BuildingStateMachine(states, transitions);
        }

        private void Update()
        {
            _buildingStateMachine?.Update();
        }
    }
}