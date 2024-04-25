using Labyrinth.Infrastructure.SaveSystem;
using UniRx;

namespace Labyrinth.Game
{
	public interface IAttemptTracker
	{
		IReadOnlyReactiveProperty<int> Attempts { get; }
		void ClearAttempts();
		void IncrementAttempts();
	}

	public class AttemptTracker : IAttemptTracker
	{
		private readonly ISaveManager _saveManager;
		private readonly ReactiveProperty<int> _attempts = new(1);

		public IReadOnlyReactiveProperty<int> Attempts => _attempts;

		public void IncrementAttempts()
		{
			_attempts.Value++;
		}

		public void ClearAttempts()
		{
			_attempts.Value = 0;
		}
	}
}