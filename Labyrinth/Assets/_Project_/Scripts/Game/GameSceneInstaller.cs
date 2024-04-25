using Labyrinth.Game.Characters.AI;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game
{
	public class GameSceneInstaller : MonoInstaller
	{
		[SerializeField] private EnemyAI _enemyPrefab;
		
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<SaveManager>().AsSingle();
		}
	}
}