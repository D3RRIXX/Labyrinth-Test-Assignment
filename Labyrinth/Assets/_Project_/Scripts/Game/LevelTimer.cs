using System;
using Labyrinth.Infrastructure.GameStateSystem;
using Labyrinth.Infrastructure.SaveSystem;
using UniRx;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game
{
	public interface ILevelTimer
	{
		IReadOnlyReactiveProperty<int> RemainingSeconds { get; }
	}

	public class LevelTimer : IInitializable, IDisposable, ILevelTimer, ISaveObserver, ILoadObserver
	{
		private readonly IGameStateManager _gameStateManager;
		private readonly ISaveManager _saveManager;
		private readonly ReactiveProperty<int> _remainingSeconds;

		private IDisposable _disposable;

		public LevelTimer(int timeLimit, IGameStateManager gameStateManager, ISaveManager saveManager)
		{
			_gameStateManager = gameStateManager;
			_saveManager = saveManager;
			_remainingSeconds = new ReactiveProperty<int>(timeLimit);
			
			saveManager.RegisterSaveObserver(this);
			saveManager.RegisterLoadObserver(this);
		}

		public IReadOnlyReactiveProperty<int> RemainingSeconds => _remainingSeconds;

		public void Initialize()
		{
			SetupTimerStream();
		}

		private void SetupTimerStream()
		{
			var levelCompletedStream = Observable.FromEvent<GameState>(h => _gameStateManager.StateChanged += h, h => _gameStateManager.StateChanged -= h)
			                                     .Where(x => x is GameState.LevelComplete or GameState.LevelFailed)
			                                     .First();

			_disposable = Observable.Interval(TimeSpan.FromSeconds(1), Scheduler.MainThreadEndOfFrame)
			                        .TakeWhile(_ => _remainingSeconds.Value > 0)
			                        .TakeUntil(levelCompletedStream)
			                        .Subscribe(_ => _remainingSeconds.Value--, () => _gameStateManager.CurrentState = GameState.LevelFailed);
		}

		public void Dispose()
		{
			_saveManager.UnregisterLoadObserver(this);
			_saveManager.UnregisterSaveObserver(this);
			_disposable?.Dispose();
		}

		public void OnSave(LevelSaveData saveData)
		{
			saveData.RemainingTime = _remainingSeconds.Value;
		}

		public void OnLoad(LevelSaveData saveData)
		{
			_remainingSeconds.Value = saveData.RemainingTime;
			if (_disposable != null)
			{
				_disposable.Dispose();
				SetupTimerStream();
			}
		}
	}
}