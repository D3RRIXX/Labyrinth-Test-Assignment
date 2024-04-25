﻿using System;
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
		void UnregisterSaveObserver(ISaveObserver observer);
		void UnregisterLoadObserver(ILoadObserver observer);
	}

	public class SaveManager : IInitializable, IDisposable, ISaveManager
	{
		public static readonly string SAVE_PATH = $"{Application.persistentDataPath}/save.dat";
		private readonly List<ISaveObserver> _saveObservers = new();
		private readonly List<ILoadObserver> _loadObservers = new();
		
		private readonly bool _loadOnInitialize;

		public LevelSaveData SaveData { get; private set; }

		public SaveManager([InjectOptional] bool loadOnInitialize)
		{
			_loadOnInitialize = loadOnInitialize;
		}

		public void Initialize()
		{
			Debug.Log("SaveManager initialize");
			if (!_loadOnInitialize || !TryLoad(out LevelSaveData saveData))
				return;

			SaveData = saveData;
			foreach (ILoadObserver observer in _loadObservers)
			{
				observer.OnLoad(SaveData);
			}
		}

		public void Dispose()
		{
			PerformSave();
		}

		public void PerformSave()
		{
			var saveData = new LevelSaveData();
			foreach (ISaveObserver observer in _saveObservers)
			{
				observer.OnSave(saveData);
			}

			File.WriteAllText(SAVE_PATH, JsonUtility.ToJson(saveData, true));
		}

		public void RegisterSaveObserver(ISaveObserver observer) => _saveObservers.Add(observer);
		public void RegisterLoadObserver(ILoadObserver observer) => _loadObservers.Add(observer);
		public void UnregisterSaveObserver(ISaveObserver observer) => _saveObservers.Remove(observer);
		public void UnregisterLoadObserver(ILoadObserver observer) => _loadObservers.Remove(observer);

		private bool TryLoad(out LevelSaveData saveData)
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