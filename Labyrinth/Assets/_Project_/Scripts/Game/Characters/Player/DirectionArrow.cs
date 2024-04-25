using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game.Characters.Player
{
	public class DirectionArrow : MonoBehaviour
	{
		[SerializeField] private GameObject _arrowRoot;
		
		private LevelFinish _levelFinish;
		private IGameStateManager _gameStateManager;

		[Inject]
		private void Construct(IGameStateManager gameStateManager)
		{
			_gameStateManager = gameStateManager;
		}
		
		private void Awake()
		{
			_gameStateManager.StateChanged += OnGameStateChanged;
		}

		private void Start()
		{
			_levelFinish = LevelFinish.Instance;
		}

		private void OnDestroy()
		{
			_gameStateManager.StateChanged -= OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState gameState)
		{
			_arrowRoot.SetActive(gameState == GameState.Gameplay);
		}

		private void Update()
		{
			Vector3 direction = _levelFinish.transform.position - transform.position;
			_arrowRoot.transform.rotation = Quaternion.LookRotation(direction);
		}
	}
}