using System;
using DI.Utiles;
using Utiles.EventSystem;
using VContainer;

namespace DI.Installers
{
    public class EventInstaller : MonoInstaller
    {
        private EventBus _eventBus;
        
        public override void Install(IContainerBuilder builder)
        {
            _eventBus = new EventBus();

            builder.RegisterInstance(_eventBus);
        }

        private void OnDestroy()
        {
            _eventBus.Dispose();
        }
    }
}