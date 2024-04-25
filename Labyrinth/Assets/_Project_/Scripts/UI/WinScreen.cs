using Labyrinth.Infrastructure.GameFlowSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.UI
{
	public class WinScreen : MonoBehaviour
	{
		private IGameFlowService _gameFlowService;

		[Inject]
		private void Construct(IGameFlowService gameFlowService)
		{
			_gameFlowService = gameFlowService;
		}

		public void LoadNextLevel()
		{
			_gameFlowService.LoadGameScene();
		}
	}
}