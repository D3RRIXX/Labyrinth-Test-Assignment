using Labyrinth.Infrastructure.GameFlowSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game
{
	public class BootLoader : MonoBehaviour
	{
		private IGameFlowService _gameFlowService;

		[Inject]
		private void Construct(IGameFlowService gameFlowService)
		{
			_gameFlowService = gameFlowService;
		}

		private void Start()
		{
			_gameFlowService.LoadGameScene();
		}
	}
}