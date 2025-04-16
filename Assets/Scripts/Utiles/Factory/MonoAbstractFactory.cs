using UnityEngine;
using Zenject;

namespace Utiles.Factory
{
    public class MonoAbstractFactory
    {
        private readonly DiContainer _container;

        public MonoAbstractFactory(DiContainer container)
        {
            _container = container;
        }

        public T Create<T>(T prefab, Transform parent = null, Vector3 position = default, Quaternion rotation = default) where T : Component
        {
            var instance = _container.InstantiatePrefabForComponent<T>(prefab, position, rotation, parent);

            return instance;
        }
    }
}