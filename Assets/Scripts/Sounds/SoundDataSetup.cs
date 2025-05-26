using System;
using System.Collections.Generic;
using UnityEngine;
using Utiles.CodGenerator;

namespace Sounds
{
    [CreateAssetMenu(fileName = "New SoundConfig", menuName = "Sounds/SoundConfig")]
    public class SoundDataSetup : ScriptableObject
    {
        [field: SerializeField] private List<SoundData> _soundDataList;

        public IReadOnlyList<SoundData> SoundDataList => _soundDataList;
        
        [ContextMenu("Generate SoundIds")]
        public void GenerateSoundIds()
        {
            SoundIDsGenerator.GenerateFromConfig(this);
        }
    }
    
    [Serializable]
    public class SoundData
    {
        [field: SerializeField] public string SoundId { get; private set; }
        [field: SerializeField] public AudioClip Sound { get; private set; }
    }
}