using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Labyrinth.UI
{
	[RequireComponent(typeof(Button))]
	public class MainMenuScreen : MonoBehaviour
	{
		private IGameStateManager _gameStateManager;

		[Inject]
		private void Construct(IGameStateManager gameStateManager)
		{
			_gameStateManager = gameStateManager;
		}
		
		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() => _gameStateManager.CurrentState = GameState.PreGame);
		}
	}
}