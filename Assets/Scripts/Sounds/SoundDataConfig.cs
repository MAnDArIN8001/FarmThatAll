using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
    public class SoundDataConfig : ScriptableObject
    {
        [field:SerializeField] public List<SoundData> SoundDataList { get; set; }
    }
    
    [Serializable]
    public class SoundData : ScriptableObject
    {
        [field: SerializeField] public SoundType Type { get; private set; }
        [field: SerializeField] public AudioClip Sound { get; private set; }
    }
}