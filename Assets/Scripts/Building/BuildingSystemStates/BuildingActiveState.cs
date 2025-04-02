using UnityEngine;
using UnityEngine.Rendering;
using Utiles.FSM;

namespace Building.BuildingSystemStates
{
    public class BuildingActiveState : State
    {
        private Building _currentBuilding;
        private BaseInput _input;
        
        private readonly BuildingStateMachine _buildingStateMachine;
        private readonly Material _validateMaterial;
        
        private GameObject _buildingShape;
        private ValidateBuilding _validateBuildingComponent;
        
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
        private bool _isValidated;

        public BuildingActiveState(StateType stateType ,BuildingStateMachine buildingStateMachine, BaseInput input)
        {
            StateType = stateType;
            
            _buildingStateMachine = buildingStateMachine;
            _validateMaterial = buildingStateMachine.ShapeMaterial;
            
            _input = input;
        }
        public override void Enter()
        {
            _buildingShape = CreateBuildingShape(_currentBuilding.BuildingPrefab);
            
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
                var validateSizeX = collider.bounds.extents.x + _currentBuilding.InteractionWithObjectsOffset;
                var validateSizeZ = collider.bounds.extents.z + _currentBuilding.InteractionWithObjectsOffset;
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
                var cursorPos = SetBuildingPositionToCursor();

                if (_isValidated)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        var building = GameObject.Instantiate(_currentBuilding.BuildingPrefab);
                        building.transform.position = cursorPos;
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
            
            var instance = GameObject.Instantiate(prefab);
            return instance;
        }

        private Vector3 SetBuildingPositionToCursor()
        {
            if (_buildingShape == null)
                return Vector3.zero;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, _buildingStateMachine.TerrainMask))
            {
                _buildingShape.transform.position = hit.point;
                return hit.point;
            }
            
            return Vector3.zero;
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
        
    }
}