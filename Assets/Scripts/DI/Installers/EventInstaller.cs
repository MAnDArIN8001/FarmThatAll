using Utiles.EventSystem;
using Zenject;

namespace DI.Installers
{
    public class EventInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EventBus>().AsSingle();
        }
    }
}