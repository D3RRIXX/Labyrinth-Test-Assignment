using Labyrinth.Infrastructure.GameStateSystem;
using UnityEngine;

namespace Labyrinth.UI
{
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;
        
        public GameState GameState => _gameState;
    }
}
