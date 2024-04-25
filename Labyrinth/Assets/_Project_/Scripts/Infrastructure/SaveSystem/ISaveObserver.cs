namespace Labyrinth.Infrastructure.SaveSystem
{
	public interface ISaveObserver
	{
		void OnSave(LevelSaveData saveData);
	}

	public interface ILoadObserver
	{
		void OnLoad(LevelSaveData saveData);
	}
}