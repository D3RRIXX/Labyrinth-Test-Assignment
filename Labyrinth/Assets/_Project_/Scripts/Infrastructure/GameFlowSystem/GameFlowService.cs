using Labyrinth.Infrastructure.SaveSystem;
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

		public event LevelStartDelegate LevelStarted;
		
		public GameFlowService(ZenjectSceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
		}

		public void LoadGameScene() => LoadGameSceneInternal(useSaveOnLoad: false);
		public void RestartLevel() => LoadGameSceneInternal(useSaveOnLoad: false);
		public void LoadSaveForCurrentLevel() => LoadGameSceneInternal(useSaveOnLoad: true);

		private void LoadGameSceneInternal(bool useSaveOnLoad)
		{
			const int gameSceneIndex = 1;
			_sceneLoader.LoadScene(gameSceneIndex, extraBindings: container => container.BindInstance(useSaveOnLoad).WhenInjectedInto<SaveManager>());
			
			LevelStarted?.Invoke(!useSaveOnLoad);
		}
	}
}