using UnityEngine;
using Utiles.EventSystem;
using Zenject;

namespace Sounds
{
    public class BackgroundSoundController : MonoBehaviour
    {
        [Inject] private SoundService _soundService;


        private void OnEnable()
        {
            _soundService.Play(SoundType.Music, "backgraund", true);   
        }
    }
}