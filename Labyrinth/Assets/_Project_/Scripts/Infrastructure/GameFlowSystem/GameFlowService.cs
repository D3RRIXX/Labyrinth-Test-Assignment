using System.Collections;
using Cysharp.Threading.Tasks;
using Labyrinth.Game;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Infrastructure.GameFlowSystem
{
	public delegate void LevelStartDelegate(bool fullRestart);

	public interface IGameFlowService
	{
		event LevelStartDelegate LevelStarted;
		void LoadGameScene();
		void RestartLevel();
		void LoadSaveForCurrentLevel();
	}

	public class GameFlowService : IGameFlowService
	{
		private readonly ZenjectSceneLoader _sceneLoader;
		private readonly IAttemptTracker _attemptTracker;

		public event LevelStartDelegate LevelStarted;
		
		public GameFlowService(ZenjectSceneLoader sceneLoader, IAttemptTracker attemptTracker)
		{
			_sceneLoader = sceneLoader;
			_attemptTracker = attemptTracker;
		}

		public void LoadGameScene() => LoadGameSceneInternalAsync(useSaveOnLoad: false, incrementAttempts: false);
		public void RestartLevel() => LoadGameSceneInternalAsync(useSaveOnLoad: false, incrementAttempts: true);
		public void LoadSaveForCurrentLevel() => LoadGameSceneInternalAsync(useSaveOnLoad: true, incrementAttempts: false);

		private async void LoadGameSceneInternalAsync(bool useSaveOnLoad, bool incrementAttempts)
		{
			const int gameSceneIndex = 1;

			await _sceneLoader.LoadSceneAsync(gameSceneIndex, extraBindings: container => container.BindInstance(useSaveOnLoad).WhenInjectedInto<SaveManager>());
			await UniTask.DelayFrame(1);
			
			if (incrementAttempts)
				_attemptTracker.Attempts.Value++;
		}
	}
}