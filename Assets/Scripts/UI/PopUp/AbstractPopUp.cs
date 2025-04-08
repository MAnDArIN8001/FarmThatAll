using System;
using UnityEngine;

namespace UI.PopUp
{
    public abstract class AbstractPopUp : MonoBehaviour
    {
        public virtual event Action<AbstractPopUp> OnPopUpOpened;
        public virtual event Action<AbstractPopUp> OnPopUpClosed;
        
        [field: SerializeField] public bool MustBeCached { get; protected set; }
        [field: SerializeField] public bool MustBeDestroyed { get; protected set; }

        public virtual void Open()
        {
            OnPopUpOpened?.Invoke(this);
            
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            OnPopUpClosed?.Invoke(this);
            
            gameObject.SetActive(false);
        }
    }
}