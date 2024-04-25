using System.Collections.Generic;
using System.Linq;
using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIScreen[] _uiScreens;
        
        private Dictionary<GameState, GameObject> _uiScreenMap;
        private IGameStateManager _gameStateManager;

        private void Reset()
        {
            _uiScreens = GetComponentsInChildren<UIScreen>(true);
        }

        [Inject]
        private void Construct(IGameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }
        
        private void Awake()
        {
            _uiScreenMap = _uiScreens.ToDictionary(x => x.GameState, x => x.gameObject);

            _gameStateManager.StateChanged += OnGameStateChanged;
        }

        private void Start()
        {
            OnGameStateChanged(GameState.Menu);
        }

        private void OnDestroy()
        {
            _gameStateManager.StateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            foreach ((GameState state, GameObject screen) in _uiScreenMap)
            {
                screen.SetActive(state == gameState);
            }
        }
    }
}
