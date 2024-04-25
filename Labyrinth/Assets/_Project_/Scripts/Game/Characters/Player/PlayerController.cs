using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        private Joystick _joystick;
        private IGameStateManager _gameStateManager;
        private bool _movementEnabled = true;

        [Inject]
        private void Construct(Joystick joystick, IGameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            _joystick = joystick;
        }
        
        private void Awake()
        {
            _gameStateManager.StateChanged += OnGameStateChanged;
        }
        
		private void OnDestroy()
		{
			_gameStateManager.StateChanged -= OnGameStateChanged;
		}

        private void OnGameStateChanged(GameState state)
        {
            _movementEnabled = state == GameState.Gameplay;
        }

        private void Update()
        {
			if (!_movementEnabled)
				return;
            
            _movement.ConsumeMovementInput(_joystick.Direction);
        }
    }
}
