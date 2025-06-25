using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sounds
{
    [CreateAssetMenu(menuName = "Setup/NewAudioMixerSetup", fileName = "NewAudioMixerSetup", order = 1)]
    public class AudioMixerSetup : ScriptableObject
    {
       [SerializeField] private List<AudioMixerData> audioMixers;

       public bool TryGetMixer(SoundType soundType, out AudioMixerGroup searchedMixerGroup)
       {
           if (audioMixers.Count == 0)
           {
               searchedMixerGroup = null;
               return false;
           }
           
           for (int i = 0; i < audioMixers.Count; i++)
           {
               if (audioMixers[i].SoundType != soundType)
               {
                   continue;
               }
               
               searchedMixerGroup = audioMixers[i].MixerGroup;
               return true;
           }
           
           searchedMixerGroup = null;
           return false;
       }
    }

    [Serializable]
    public class AudioMixerData
    {
        [field: SerializeField] public SoundType SoundType { get; private set; }
        
        [field: SerializeField] public AudioMixerGroup MixerGroup { get; private set; }
    }
}