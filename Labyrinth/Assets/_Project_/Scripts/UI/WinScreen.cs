using Labyrinth.Game;
using Labyrinth.Infrastructure.GameFlowSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.UI
{
	public class WinScreen : MonoBehaviour
	{
		private IGameFlowService _gameFlowService;
		private IAttemptTracker _attemptTracker;

		[Inject]
		private void Construct(IGameFlowService gameFlowService, IAttemptTracker attemptTracker)
		{
			_attemptTracker = attemptTracker;
			_gameFlowService = gameFlowService;
		}

		public void LoadNextLevel()
		{
			_attemptTracker.ClearAttempts();
			_gameFlowService.LoadGameScene();
		}
	}
}