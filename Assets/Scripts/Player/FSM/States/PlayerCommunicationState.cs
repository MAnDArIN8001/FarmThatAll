using Communication;
using DG.Tweening;
using Player.CameraControls;
using Player.Controls;
using Player.Setups;
using UnityEngine;
using Utiles.FSM;
using CameraType = Player.CameraControls.CameraType;

namespace Player.FSM.States
{
    public class PlayerCommunicationState : State
    {
        private readonly PointerSystem _pointerSystem;
        private readonly CameraSystem _cameraSystem;

        private readonly Transform _contextTransform;
        
        private readonly AnimationSetup _rotationAnimationSetup;

        private ICommunicable _communicable;
        
        public PlayerCommunicationState(StateType stateType, PointerSystem pointerSystem, 
            CameraSystem cameraSystem, Transform context, AnimationSetup rotationAnimationSetup)
        {
            StateType = stateType;
            _pointerSystem = pointerSystem;
            _cameraSystem = cameraSystem;
            _contextTransform = context;
            _rotationAnimationSetup = rotationAnimationSetup;
        }
        
        public override void Enter()
        {
            RotateTowardsCommunicable();
            
            _communicable = _pointerSystem.PointedCommunicable;
            
            _communicable.Communicate();
            
            _cameraSystem.SetCamera(CameraType.FirstPerson);
        }

        public override void Update() { }

        public override void Exit()
        {
            _communicable.CloseCommunication();
            
            _cameraSystem.SetCamera(CameraType.ThirdPerson);
        }

        private void RotateTowardsCommunicable()
        {
            Vector3 communicablePosition = _pointerSystem.PointedCommunicable.CommunicationViewpointTransform.position;
            Vector3 direction = (communicablePosition - _contextTransform.position).normalized;
            
            Quaternion directionalRotation = Quaternion.LookRotation(direction);

            directionalRotation.x = 0;
            directionalRotation.z = 0;

            _contextTransform.DORotate(directionalRotation.eulerAngles, _rotationAnimationSetup.AnimationTime)
                .SetEase(_rotationAnimationSetup.AnimationEase);
        }
    }
}