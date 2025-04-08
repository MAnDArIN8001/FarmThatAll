using System;
using DG.Tweening;
using UnityEngine;

namespace Player.Setups
{
    [Serializable]
    public class AnimationSetup
    {
        [field: SerializeField] public float AnimationTime { get; private set; }
        
        [field: SerializeField, Space] public Ease AnimationEase { get; private set; }
    }
}