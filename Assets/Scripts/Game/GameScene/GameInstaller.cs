using Zenject;

namespace Game.GameScene
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShootingController>().FromComponentInHierarchy().AsSingle();
        }
    }
}