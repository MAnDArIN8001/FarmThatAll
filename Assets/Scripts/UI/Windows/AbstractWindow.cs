using System;
using UnityEngine;

namespace UI.Windows
{
    public abstract class AbstractWindow : MonoBehaviour
    {
        public virtual void Open(Action callBack = null)
        {
            gameObject.SetActive(true);
            
            callBack?.Invoke();
        }

        public virtual void Close(Action callBack = null)
        {
            gameObject.SetActive(false);
            
            callBack?.Invoke();
        }
    }
}