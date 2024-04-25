using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth
{
	public class LevelFinish : MonoBehaviour
	{
		private IGameStateManager _gameStateManager;
		public static LevelFinish Instance { get; private set; }

		[Inject]
		private void Construct(IGameStateManager gameStateManager)
		{
			_gameStateManager = gameStateManager;
		}
		
		private void Awake()
		{
			Instance = this;
			Debug.Log(Application.persistentDataPath);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
				_gameStateManager.CurrentState = GameState.LevelComplete;
		}
	}
}