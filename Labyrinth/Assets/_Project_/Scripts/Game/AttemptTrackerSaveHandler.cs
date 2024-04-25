using System;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;

namespace Labyrinth.Game
{
	public class AttemptTrackerSaveHandler : IDisposable, ISaveObserver, ILoadObserver
	{
		private readonly IAttemptTracker _attemptTracker;
		private readonly ISaveManager _saveManager;

		public AttemptTrackerSaveHandler(IAttemptTracker attemptTracker, ISaveManager saveManager)
		{
			_attemptTracker = attemptTracker;
			_saveManager = saveManager;
			
			_saveManager.RegisterSaveObserver(this);
			_saveManager.RegisterLoadObserver(this);
		}

		public void Dispose()
		{
			_saveManager.UnregisterSaveObserver(this);
			_saveManager.UnregisterLoadObserver(this);
		}

		public void OnSave(LevelSaveData saveData)
		{
			saveData.Attempts = _attemptTracker.Attempts.Value;
		}

		public void OnLoad(LevelSaveData saveData)
		{
			_attemptTracker.Attempts.Value = saveData.Attempts;
		}
	}
}