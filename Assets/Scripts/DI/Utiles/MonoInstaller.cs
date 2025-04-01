using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI.Utiles
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public virtual void Install(IContainerBuilder builder) { }
    }
}