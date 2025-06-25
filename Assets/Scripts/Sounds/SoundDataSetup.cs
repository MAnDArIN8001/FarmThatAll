using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sounds
{
    [CreateAssetMenu(menuName = "Setup/SoundDataSetup", fileName = "NewSoundDataSetup", order = 1)]
    public class SoundDataSetup : ScriptableObject
    {
        [SerializeField] private List<SoundData> soundDataList;
        public IReadOnlyList<SoundData> SoundDataList => soundDataList;
    }
    
    [Serializable]
    public class SoundData
    {
        [field: SerializeField] public string Type { get; private set; }
        
        [field: SerializeField] public List<AudioClip> Sound { get; private set; }
    }
}