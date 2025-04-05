using System;
using UnityEngine;
using Utiles.EventSystem;
using Utiles.FSM;
using State = Utiles.FSM.State;

namespace Building.BuildingSystemStates
{
    public class BuildingActiveState : State
    {
        private bool _isValidated;
        
        private readonly BaseInput _input;
        
        private readonly EventBus _eventBus;
        
        private readonly Material _validateMaterial;
        
        private GameObject _buildingShape;
        
        private ValidateBuilding _validateBuildingComponent;
        
        private BuildingData _currentBuildingData;
        public BuildingData CurrentBuildingData
        {
            get => _currentBuildingData;
            set
            {
                if (_currentBuildingData == value)
                    return;
                
                _currentBuildingData = value;
                _buildingShape = CreateBuildingShape(_currentBuildingData.BuildingPrefab);
            }
        }
        
        private Color _validateColor;
        private Color ValidateColor
        {
            get => _validateColor;
            set
            {
                if (_validateColor == value)
                    return;
               
                _validateColor = value;
                
                _validateMaterial.color = _validateColor;
            }
        }
        
        public BuildingActiveState(StateType stateType, EventBus eventBus, Material shapeMaterial ,BaseInput input)
        {
            StateType = stateType;
            
            _input = input;
            
            _eventBus = eventBus;
            
            _eventBus.Subscribe<BuildingData>(HandleBuildingChanging);
            
            _validateMaterial = shapeMaterial;
        }
        
        public override void Enter()
        {
            _buildingShape = CreateBuildingShape(_currentBuildingData.BuildingPrefab);
            
            if (_buildingShape.TryGetComponent(out MeshRenderer shapeMeshRenderer))
            {
                ReplaceMeshRendererMaterial(shapeMeshRenderer, _validateMaterial);
            }
            else
            {
                Debug.LogError("No mesh renderer found");
            }
            
            if (_buildingShape.TryGetComponent(out BoxCollider collider))
            {
                var validateSizeX = collider.bounds.extents.x + _currentBuildingData.InteractionWithObjectsOffset;
                
                var validateSizeZ = collider.bounds.extents.z + _currentBuildingData.InteractionWithObjectsOffset;
                
                collider.isTrigger = true;
                
                collider.size = new Vector3(validateSizeX * 2, collider.size.y, validateSizeZ * 2);
                
                _validateBuildingComponent = _buildingShape.AddComponent<ValidateBuilding>();
                
                var rigidbody = _buildingShape.AddComponent<Rigidbody>();
                
                rigidbody.isKinematic = true;
            }
            else
            {
                Debug.LogError("No collider found");
            }
            
            _validateBuildingComponent.OnToggleValidity += ValidateBuildingPosition;
        }

        public override void Update()
        {
            if (_buildingShape != null)
            {
                SetBuildingPositionToCursor();

                if (_isValidated)
                {
                    if (_input.Mouse.Click.WasPerformedThisFrame())
                    {
                        var building = GameObject.Instantiate(_currentBuildingData.BuildingPrefab);
                        building.transform.position = _buildingShape.transform.position;
                    }
                }
            }
        }

        public override void Exit()
        {
            _validateBuildingComponent.OnToggleValidity -= ValidateBuildingPosition;
            
            _validateBuildingComponent = null;
            
            GameObject.Destroy(_buildingShape);
            
            _buildingShape = null;
        }

        private GameObject CreateBuildingShape(GameObject prefab)
        {
            if (prefab == null)
                return null;
            
            var buildingShape = new GameObject("BuildingShape");
            
            if (prefab.TryGetComponent<MeshFilter>(out var prefabMeshFilter))
            {
                if (prefabMeshFilter.sharedMesh != null)
                {
                    var shapeMeshFilter = buildingShape.AddComponent<MeshFilter>();
                    shapeMeshFilter.sharedMesh = prefabMeshFilter.sharedMesh;
                }
                else
                {
                    Debug.LogWarning(prefab.name + " has no Mesh in MeshFilter!");
                }
            }
            else
            {
                Debug.LogWarning(prefab.name + " has no MeshFilter!");
            }
            
            if (prefab.TryGetComponent<MeshRenderer>(out var prefabMeshRenderer))
            {
                if (prefabMeshFilter.sharedMesh != null)
                {
                    var shapeMeshRenderer = buildingShape.AddComponent<MeshRenderer>();
                    shapeMeshRenderer.sharedMaterials = new Material[prefabMeshRenderer.sharedMaterials.Length];
                }
                else
                {
                    Debug.LogWarning(prefab.name + " has no MeshRenderer!");
                }
            }
            else
            {
                Debug.LogWarning(prefab.name + " has no MeshFilter!");
            }
            
            if (prefab.TryGetComponent<BoxCollider>(out var prefabCollider))
            {
                var shapeCollider = buildingShape.AddComponent<BoxCollider>();
                
                shapeCollider.center = prefabCollider.center;
                shapeCollider.size = prefabCollider.size;
                shapeCollider.isTrigger = prefabCollider.isTrigger;
            }
            else
            {
                Debug.LogWarning(prefab.name + " has no BoxCollider!");
            }
            
            buildingShape.transform.localScale = prefab.transform.localScale;
            
            return buildingShape;
        }

        private void SetBuildingPositionToCursor()
        {
            if (_buildingShape == null)
                return;
            
            Ray ray = Camera.main.ScreenPointToRay(_input.Mouse.Position.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, _currentBuildingData.TerrainLayerMask))
            {
                _buildingShape.transform.position = hit.point;
            }
        }
        
        private void ValidateBuildingPosition(bool isToggled)
        {
            ValidateColor = isToggled ? Color.green : Color.red;
            
            _isValidated = isToggled; 
        }

        private void ReplaceMeshRendererMaterial(MeshRenderer renderer, Material material)
        {
            var materials = new Material[renderer.sharedMaterials.Length];
            
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = material;
            }
            
            renderer.materials = materials;
        }
        
        private void HandleBuildingChanging(BuildingData buildingData)
        {
            _currentBuildingData = buildingData;
            
            Debug.Log($"BuildingChanged: {buildingData.Name}");
        }
    }
}