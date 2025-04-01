using System;
using Building.BuildingSystemStates;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Building
{
    public class BuildingSystem : MonoBehaviour
    {
        //Переделать на 1 билдинг, который он получает из вне
        [SerializeField] private BuildingsData buildingsData;
        [field:SerializeField] public LayerMask LayerMaskForBuilding {get; private set;}
        [field:SerializeField] public LayerMask LayerMaskForValidateBuilding {get; private set;}
        [field:SerializeField] public Material ValidateObjectMaterial { get; set; }
        
        private BuildingSystemBaseState _currentState;
        public BuildingSystemIdleState IdleState = new();
        public BuildingSystemPlaceSelectionState PlaceSelectionState = new();
        
        public Camera mainCamera;
        public Building currentBuilding;
        private void OnEnable()
        {
            mainCamera = Camera.main;
            SwitchState(IdleState);
        }
        
        private void Update()
        {
            _currentState?.UpdateState();
        }

        public void SwitchState(BuildingSystemBaseState newState)
        {
            _currentState?.ExitState(this);
            _currentState = newState;
            _currentState?.EnterState(this);
        }
        
        public void SetBuilding(int buildingId)
        {
            if (buildingId < 0 || buildingId >= buildingsData.Buildings.Count)
                return;
            
            currentBuilding = buildingsData.Buildings[buildingId];
            
            SwitchState(PlaceSelectionState);
        }
    }
}