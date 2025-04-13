using Utiles.Factory;
using Zenject;

namespace DI.Installers
{
    public class FactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MonoAbstractFactory>().AsSingle();
        }
    }
}