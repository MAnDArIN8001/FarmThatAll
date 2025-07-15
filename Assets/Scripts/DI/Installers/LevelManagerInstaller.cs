using Utiles;
using Zenject;

namespace DI.Installers
{
    public class LevelManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelManager>().FromNew().AsSingle().NonLazy();
        }
    }
}