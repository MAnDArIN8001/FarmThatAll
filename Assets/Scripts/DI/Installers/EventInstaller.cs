using Utiles.EventSystem;
using Zenject;

namespace DI.Installers
{
    public class EventInstaller : MonoInstaller
    {
        private EventBus _eventBus;

        public override void InstallBindings()
        {
            _eventBus = new EventBus();
            
            Container.Bind<EventBus>().FromInstance(_eventBus).AsSingle().NonLazy();
        }

        private void OnDestroy()
        {
            _eventBus.Dispose();
        }
    }
}