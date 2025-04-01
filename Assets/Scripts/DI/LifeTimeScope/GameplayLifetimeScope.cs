using System.Collections.Generic;
using DI.Installers;
using DI.Utiles;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI.LifeTimeScope
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<MonoInstaller> _monoInstallers;

        protected override void Configure(IContainerBuilder builder)
        {
            InitializeMonoInstallers(builder);
        }

        private void InitializeMonoInstallers(IContainerBuilder builder)
        {
            foreach (var monoInstaller in _monoInstallers)
            {
                monoInstaller.Install(builder);
                
                
            }
        }
    }
}