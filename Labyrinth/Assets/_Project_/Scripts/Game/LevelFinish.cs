using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth
{
	[RequireComponent(typeof(BoxCollider))]
	public class LevelFinish : MonoBehaviour
	{
		private IGameStateManager _gameStateManager;

		[Inject]
		private void Construct(IGameStateManager gameStateManager)
		{
			_gameStateManager = gameStateManager;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
				_gameStateManager.CurrentState = GameState.LevelComplete;
		}
	}
}