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

        public bool TryGetSoundData(string soundType, out SoundData soundData)
        {
            if (SoundDataList.Count <= 0)
            {
                soundData = null;
                return false;
            }

            for (int i = 0; i < soundDataList.Count; i++)
            {
                if (soundDataList[i].Type != soundType)
                {
                    continue;
                }
                
                soundData = SoundDataList[i];
                return true;
            }
            
            soundData = null;
            return false;
        }
    }
    
    [Serializable]
    public class SoundData
    {
        [field: SerializeField] public string Type { get; private set; }
        
        [field: SerializeField] public List<AudioClip> Sound { get; private set; }
    }
}