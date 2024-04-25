using System;
using Labyrinth.Infrastructure.GameFlowSystem;
using Labyrinth.Infrastructure.SaveSystem;

namespace Labyrinth.Game
{
	public interface IAttemptTracker
	{
		int Attempts { get; }
	}

	public class AttemptTracker : IDisposable, ISaveObserver, ILoadObserver, IAttemptTracker
	{
		private readonly IGameFlowService _gameFlowService;
		private readonly ISaveManager _saveManager;
		
		public int Attempts { get; private set; }
		
		public AttemptTracker(ISaveManager saveManager, IGameFlowService gameFlowService)
		{
			_saveManager = saveManager;
			_gameFlowService = gameFlowService;
			_gameFlowService.LevelStarted += OnLevelStarted;
			
			saveManager.RegisterSaveObserver(this);
			saveManager.RegisterLoadObserver(this);
		}

		public void Dispose()
		{
			_saveManager.UnregisterLoadObserver(this);
			_saveManager.UnregisterSaveObserver(this);
			
			_gameFlowService.LevelStarted -= OnLevelStarted;
		}

		private void OnLevelStarted(bool fullRestart)
		{
			if (fullRestart)
				Attempts++;
		}

		public void OnSave(LevelSaveData saveData)
		{
			saveData.Attempts = Attempts;
		}

		public void OnLoad(LevelSaveData saveData)
		{
			Attempts = saveData.Attempts;
		}
	}
}