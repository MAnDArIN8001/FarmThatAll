using Storage;
using Storage.Setup;
using UnityEngine;
using Zenject;

namespace DI.Installers
{
    public class StorageInstaller : MonoInstaller
    {
        [SerializeField] private ItemsSetup _itemsSetup;

        public override void InstallBindings()
        {
            Container.BindInstance(_itemsSetup);
            
            Container.BindInterfacesAndSelfTo<Storage.Storage>().AsSingle();
        }
    }
}