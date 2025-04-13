using System;
using System.Collections.Generic;
using System.Linq;
using UI.PopUp;
using UnityEngine;

namespace Utiles.Services
{
    public class PopUpService : IDisposable
    {
        private const string _popUpDirectoryPath = "UI/PopUp/";

        private readonly Canvas _globalCanvas;
        
        private readonly Dictionary<Type, AbstractPopUp> _cachedPopUps = new();
        private readonly Dictionary<Type, AbstractPopUp> _activePopUps = new();
        private readonly Dictionary<Type, AbstractPopUp> _inactivePopUps = new();

        public PopUpService(Canvas globalCanvas)
        {
            _globalCanvas = globalCanvas;
        }

        public bool TryGetPopUp<T>(out T popUp) where T : AbstractPopUp
        { 
            var typeOfPopUp = typeof(T);

            if (_activePopUps.TryGetValue(typeOfPopUp, out var activePopUp))
            {
                popUp = (T)activePopUp;

                return true;
            }

            if (_inactivePopUps.TryGetValue(typeOfPopUp, out var inactivePopUp))
            {
                popUp = (T)inactivePopUp;

                return true;
            }

            if (_cachedPopUps.TryGetValue(typeOfPopUp, out var cachedPopUp))
            {
               popUp = (T)GameObject.Instantiate(cachedPopUp, _globalCanvas.transform);;
               
               return true;
            }
            
            var pathToPopUp = _popUpDirectoryPath + typeOfPopUp.Name;

            var popUpFromResources = Resources.Load<T>(pathToPopUp);

            if (popUpFromResources is not null)
            {
                if (popUpFromResources.MustBeCached)
                {
                    _cachedPopUps.Add(typeOfPopUp, popUpFromResources);    
                }   
                
                popUp = GameObject.Instantiate(popUpFromResources, _globalCanvas.transform);

                return true;
            }
            
            popUp = null;

            return false;
        }

        public void OpenPopUp<T>(Vector3 position, out T popUp) where T : AbstractPopUp
        {
            var typeOfPopUp = typeof(T);

            if (_activePopUps.TryGetValue(typeOfPopUp, out var activePopUp))
            {
                popUp = (T)activePopUp;
                
                Debug.LogWarning($"The Pop Up {typeOfPopUp} has already been opened");
                
                return;
            }
            
            if (TryGetPopUp(out T popUpInstance))
            {
                popUpInstance.OnPopUpOpened += HandlePopUpOpened;
                popUpInstance.OnPopUpClosed += HandlePopUpClosed;
                
                popUpInstance.Open();
                
                popUpInstance.transform.position = position;

                popUp = popUpInstance;
                
                return;
            }

            popUp = null;
        }

        public void ClosePopUp<T>() where T : AbstractPopUp
        {
            var typeOfPopUp = typeof(T);
            
            if (_activePopUps.TryGetValue(typeOfPopUp, out var activePopUp))
            {
                activePopUp.Close();
            }
        }

        private void HandlePopUpOpened(AbstractPopUp popUp)
        {
            var typeOfPopUp = popUp.GetType();
            
            if (_inactivePopUps.ContainsValue(popUp))
            {
                _inactivePopUps.Remove(typeOfPopUp);
            }

            _activePopUps.Add(typeOfPopUp, popUp);
            
            popUp.OnPopUpOpened -= HandlePopUpOpened;
        }

        private void HandlePopUpClosed(AbstractPopUp popUp)
        {
            var typeOfPopUp = popUp.GetType();
            
            if (!popUp.MustBeDestroyed)
            {
                _activePopUps.Remove(typeOfPopUp);
                _inactivePopUps.Add(typeOfPopUp, popUp);
                
                popUp.OnPopUpOpened += HandlePopUpOpened;
            }
            
            popUp.OnPopUpClosed -= HandlePopUpClosed;
        }

        public void Dispose()
        {
            foreach (var popUp in _activePopUps.Values)
            {
                popUp.OnPopUpClosed -= HandlePopUpClosed;
            }

            foreach (var popUp in _inactivePopUps.Values)
            {
                popUp.OnPopUpOpened -= HandlePopUpOpened;
            }
            
            _cachedPopUps.Clear();
            _activePopUps.Clear();
            _inactivePopUps.Clear();
        }
    }
}