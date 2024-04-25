using Labyrinth.Infrastructure.SaveSystem;
using UniRx;

namespace Labyrinth.Game
{
	public interface IAttemptTracker
	{
		ReactiveProperty<int> Attempts { get; }
		public void ClearAttempts();
	}

	public class AttemptTracker : IAttemptTracker
	{
		private const int INITIAL_VALUE = 1;
		
		private readonly ISaveManager _saveManager;

		public ReactiveProperty<int> Attempts { get; } = new(INITIAL_VALUE);

		public void ClearAttempts()
		{
			Attempts.Value = INITIAL_VALUE;
		}
	}
}