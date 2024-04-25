using Labyrinth.Game;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Labyrinth.UI
{
	public class HudScreen : MonoBehaviour
	{
		[SerializeField] private TMP_Text _attemptsLabel;
		[SerializeField] private TMP_Text _timerLabel;
		[Header("Pause Menu")]
		[SerializeField] private GameObject _pauseMenu;

		[Inject]
		private void Construct(IAttemptTracker attemptTracker,ILevelTimer levelTimer)
		{
			attemptTracker.Attempts
			              .Subscribe(x => _attemptsLabel.text = $"Attempts: {x}")
			              .AddTo(this);

			levelTimer.RemainingSeconds
			          .Subscribe(x => _timerLabel.text = $"Time Left: {x}")
			          .AddTo(this);
		}

		public void PauseGame()
		{
			_pauseMenu.SetActive(true);
			Time.timeScale = 0f;
		}

		public void UnpauseGame()
		{
			_pauseMenu.SetActive(false);
			Time.timeScale = 1f;
		}
	}
}