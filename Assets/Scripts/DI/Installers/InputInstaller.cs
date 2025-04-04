

using Zenject;

namespace DI.Installers
{
    public class InputInstaller : MonoInstaller
    {
        private BaseInput _baseInput;

        public override void InstallBindings()
        {
            _baseInput = new BaseInput();
            _baseInput.Enable();

            Container.Bind<BaseInput>().FromInstance(_baseInput);
        }

        public void OnDestroy()
        {
            _baseInput.Disable();
        }
    }
}