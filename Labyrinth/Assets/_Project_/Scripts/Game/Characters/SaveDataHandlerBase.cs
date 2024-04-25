using Labyrinth.Infrastructure.SaveSystem;
using UnityEngine;
using Zenject;

namespace Labyrinth.Game.Characters
{
	public abstract class SaveDataHandlerBase : MonoBehaviour, ISaveObserver, ILoadObserver
	{
		protected ISaveManager SaveManager { get; private set; }

		[Inject]
		private void Construct(ISaveManager saveManager)
		{
			SaveManager = saveManager;
			saveManager.RegisterSaveObserver(this);
			saveManager.RegisterLoadObserver(this);
		}

		protected virtual void OnDestroy()
		{
			SaveManager.UnregisterLoadObserver(this);
			SaveManager.UnregisterSaveObserver(this);
		}

		public abstract void OnSave(LevelSaveData saveData);
		public abstract void OnLoad(LevelSaveData saveData);
	}
}