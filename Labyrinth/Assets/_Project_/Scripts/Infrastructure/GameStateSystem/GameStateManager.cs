using System;
using UnityEngine;
using Zenject;

namespace Labyrinth.Infrastructure.GameStateSystem
{
	public interface IGameStateManager
	{
		GameState CurrentState { get; set; }
		event Action<GameState> StateChanged;
	}

	public class GameStateManager : IInitializable, IGameStateManager
	{
		private GameState currentState;

		public event Action<GameState> StateChanged;
		
		public GameState CurrentState
		{
			get => currentState;
			set
			{
				if (currentState == value)
					return;

				Debug.Log($"Game State changed to {value}");
				currentState = value;
				StateChanged?.Invoke(value);
			}
		}

		public void Initialize()
		{
			CurrentState = GameState.Gameplay;
		}
	}
}