using Labyrinth.Infrastructure.GameFlowSystem;
using Zenject;

namespace Labyrinth.Infrastructure
{
	public class BootstrapInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<GameFlowService>().AsSingle();
		}
	}
}