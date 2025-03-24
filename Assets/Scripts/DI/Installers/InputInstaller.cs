using System;
using VContainer;
using VContainer.Unity;

namespace DI.Installers
{
    public class InputInstaller : IInstaller
    {
        private BaseInput _baseInput;
        
        public void Install(IContainerBuilder builder)
        {
            _baseInput = new BaseInput();
            _baseInput.Enable();

            builder.RegisterInstance(_baseInput);
        }

        public void Dispose()
        {
            _baseInput.Disable();
        }
    }
}