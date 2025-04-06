using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Player.CameraControls
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private CameraType _defaultCamera;
        
        [Space, SerializeField] private List<CameraPair> _cameraPairs;

        private CameraPair _currentCameraPair;

        private void Awake()
        {
            SetCamera(_defaultCamera);
        }

        public void SetCamera(CameraType cameraType)
        {
            var cameraPair = _cameraPairs.Find((cameraPair) => cameraPair.CameraType == cameraType);

            if (cameraPair is null)
            {
                Debug.LogError($"Camera System {this} doesnt contains camera with type {cameraType}");

                return;
            }

            cameraPair.CinemachineCamera.gameObject.SetActive(true);
            cameraPair.CinemachineCamera.Priority = (int)CameraPriority.High;

            if (_currentCameraPair is not null)
            {
                _currentCameraPair.CinemachineCamera.gameObject.SetActive(false);
                _currentCameraPair.CinemachineCamera.Priority = (int)CameraPriority.Low;
            } 
            
            _currentCameraPair = cameraPair;
        }

        private void OnDestroy()
        {
            _cameraPairs.Clear();
        }
    }

    [Serializable]
    public class CameraPair
    {
        [field: SerializeField] public CameraType CameraType { get; private set; }

        [field: SerializeField] public CinemachineCamera CinemachineCamera { get; private set; }
    }
}