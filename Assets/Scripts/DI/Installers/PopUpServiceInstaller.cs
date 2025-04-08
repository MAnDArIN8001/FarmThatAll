using System;
using UnityEngine;
using Utiles.Services;
using Zenject;

namespace DI.Installers
{
    public class PopUpServiceInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _globalCanvas;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PopUpService>().FromNew().AsSingle().WithArguments(_globalCanvas);
        }
    }
}