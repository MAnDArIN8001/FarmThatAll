using System;
using Sounds;
using UnityEngine;
using Zenject;

namespace UI.PopUp
{
    public abstract class AbstractPopUp : MonoBehaviour
    {
        public virtual event Action<AbstractPopUp> OnPopUpOpened;
        public virtual event Action<AbstractPopUp> OnPopUpClosed;

        [Inject] private SoundService _soundService;
        [field: SerializeField] public bool MustBeCached { get; protected set; }
        [field: SerializeField] public bool MustBeDestroyed { get; protected set; }

        public virtual void Open()
        {
            OnPopUpOpened?.Invoke(this);
            
            gameObject.SetActive(true);

            _soundService.Play(SoundType.Music, "openPopUp");
        }

        public virtual void Close()
        {
            OnPopUpClosed?.Invoke(this);
            
            _soundService.Play(SoundType.Music, "closePopUp");
            gameObject.SetActive(false);
        }
    }
}