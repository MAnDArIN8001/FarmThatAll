using UnityEngine;

namespace Player
{
    public class ControllableAnimator : MonoBehaviour
    {
        [Header("Animations Keys")] 
        [SerializeField] private string _movementAnimationKey;
        [SerializeField] private string _idleAnimationKey;
        [SerializeField] private string _communicateAnimationKey;
        
        [Space, SerializeField] private Animator _animator;

        public void PlayIdle()
        {
            _animator.SetTrigger(_idleAnimationKey);
        }
        
        public void SetWalk(bool value)
        {
            _animator.SetBool(_movementAnimationKey, value);
        }

        public void PlayCommunicate()
        {
            _animator.SetTrigger(_communicateAnimationKey);
        }
    }
}