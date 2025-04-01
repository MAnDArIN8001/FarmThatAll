using UnityEngine;
using UnityEngine.Rendering;

namespace Building.BuildingSystemStates
{
    public class BuildingSystemPlaceSelectionState : BuildingSystemBaseState
    {
        private BuildingSystem _buildingSystem;
        private GameObject _buildingShape;

        private ValidateBuilding _validateBuildingComponent;
        private Material _validateMaterial;
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
        private bool _isValidated = true;
        private bool IsValidated
        {
            get => _isValidated;
            set
            {
                if (_isValidated == value)
                    return;
                
                _isValidated = value;
            }
        }
        public override void EnterState(BuildingSystem buildingSystem)
        {
            _buildingSystem = buildingSystem;
            _validateMaterial = buildingSystem.ValidateObjectMaterial;

            _buildingShape = CreateBuildingShape(_buildingSystem.currentBuilding.BuildingPrefab);
            
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
                var validateSizeX = collider.bounds.extents.x + buildingSystem.currentBuilding.InteractionWithObjectsOffset;
                var validateSizeZ = collider.bounds.extents.z + buildingSystem.currentBuilding.InteractionWithObjectsOffset;
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

        public override void UpdateState()
        {
            if (_buildingShape != null)
            {
                var cursorPos = SetBuildingPositionToCursor();

                if (IsValidated)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        var building = GameObject.Instantiate(_buildingSystem.currentBuilding.BuildingPrefab);
                        building.transform.position = cursorPos;
                        
                        _buildingSystem.SwitchState(_buildingSystem.IdleState);
                    }
                }
                
            }
        }

        public override void ExitState(BuildingSystem buildingSystem)
        {
            _validateBuildingComponent.OnToggleValidity -= ValidateBuildingPosition;
            _validateBuildingComponent = null;
            _buildingSystem = null;
            
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
            
            Ray ray = _buildingSystem.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, _buildingSystem.LayerMaskForBuilding))
            {
                _buildingShape.transform.position = hit.point;
                return hit.point;
            }
            
            return Vector3.zero;
        }
        
        private void ValidateBuildingPosition(bool isToggled)
        {
            ValidateColor = isToggled ? Color.green : Color.red;
            IsValidated = isToggled; 
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