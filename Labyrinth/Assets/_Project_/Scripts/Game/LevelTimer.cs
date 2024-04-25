using System;
using Labyrinth.Infrastructure.GameStateSystem;
using UniRx;
using Zenject;

namespace Labyrinth.Game
{
	public interface ILevelTimer
	{
		IReadOnlyReactiveProperty<int> RemainingSeconds { get; }
	}

	public class LevelTimer : IInitializable, IDisposable, ILevelTimer
	{
		private readonly IGameStateManager _gameStateManager;
		private readonly ReactiveProperty<int> _remainingSeconds;

		private IDisposable _disposable;

		public LevelTimer(int timeLimit, IGameStateManager gameStateManager)
		{
			_gameStateManager = gameStateManager;
			_remainingSeconds = new ReactiveProperty<int>(timeLimit);
		}

		public IReadOnlyReactiveProperty<int> RemainingSeconds => _remainingSeconds;

		public void Initialize()
		{
			var levelCompletedStream = Observable.FromEvent<GameState>(h => _gameStateManager.StateChanged += h, h => _gameStateManager.StateChanged -= h)
			                                     .Where(x => x is GameState.LevelComplete)
			                                     .First();

			_disposable = Observable.Interval(TimeSpan.FromSeconds(1), Scheduler.MainThread)
			                        .TakeWhile(_ => _remainingSeconds.Value > 0)
			                        .TakeUntil(levelCompletedStream)
			                        .Subscribe(_ => _remainingSeconds.Value--, () => _gameStateManager.CurrentState = GameState.LevelFailed);
		}

		public void Dispose()
		{
			_disposable?.Dispose();
		}
	}
}