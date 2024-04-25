using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace Labyrinth.Infrastructure.SaveSystem
{
	public interface ISaveManager
	{
		LevelSaveData SaveData { get; }
		void PerformSave();
		void RegisterSaveObserver(ISaveObserver observer);
		void RegisterLoadObserver(ILoadObserver observer);
	}

	public class SaveManager : IInitializable, ISaveManager
	{
		private static readonly string SAVE_PATH = $"{Application.persistentDataPath}/save.dat";
		private readonly List<ISaveObserver> _saveObservers = new();
		private readonly List<ILoadObserver> _loadObservers = new();

		public LevelSaveData SaveData { get; private set; }

		public void Initialize()
		{
			if (!Load(out LevelSaveData saveData))
				return;

			SaveData = saveData;
			foreach (ILoadObserver observer in _loadObservers)
			{
				observer.OnLoad(SaveData);
			}
		}

		public void PerformSave()
		{
			var saveData = new LevelSaveData();
			foreach (ISaveObserver observer in _saveObservers)
			{
				observer.OnSave(saveData);
			}

			File.WriteAllText(SAVE_PATH, JsonUtility.ToJson(saveData));
		}

		public void RegisterSaveObserver(ISaveObserver observer)
		{
			_saveObservers.Add(observer);
		}

		public void RegisterLoadObserver(ILoadObserver observer)
		{
			_loadObservers.Add(observer);
		}

		private bool Load(out LevelSaveData saveData)
		{
			if (!File.Exists(SAVE_PATH))
			{
				saveData = null;
				return false;
			}

			string json = File.ReadAllText(SAVE_PATH);
			saveData = JsonUtility.FromJson<LevelSaveData>(json);
			return true;
		}
	}
}