using System;
using DI.Utiles;
using VContainer;
using VContainer.Unity;

namespace DI.Installers
{
    public class InputInstaller : MonoInstaller
    {
        private BaseInput _baseInput;
        
        public override void Install(IContainerBuilder builder)
        {
            _baseInput = new BaseInput();
            _baseInput.Enable();

            builder.RegisterInstance(_baseInput);
        }

        public void OnDestroy()
        {
            _baseInput.Disable();
        }
    }
}