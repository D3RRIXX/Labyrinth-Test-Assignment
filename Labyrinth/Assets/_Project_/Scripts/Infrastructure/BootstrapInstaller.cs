using Labyrinth.Game;
using Labyrinth.Infrastructure.GameFlowSystem;
using Zenject;

namespace Labyrinth.Infrastructure
{
	public class BootstrapInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<AttemptTracker>().AsSingle();
			Container.BindInterfacesTo<GameFlowService>().AsSingle();
		}
	}
}