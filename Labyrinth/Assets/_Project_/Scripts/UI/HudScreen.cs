using System.Collections;
using Labyrinth.Game;
using Labyrinth.Infrastructure.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Labyrinth.UI
{
	public class HudScreen : MonoBehaviour
	{
		[SerializeField] private TMP_Text _attemptsLabel;
		[Header("Pause Menu")]
		[SerializeField] private GameObject _pauseMenu;

		private ISaveManager _saveManager;
		private IAttemptTracker _attemptTracker;

		[Inject]
		private void Construct(IAttemptTracker attemptTracker, ISaveManager saveManager)
		{
			_attemptTracker = attemptTracker;
			_saveManager = saveManager;
		}

		private void OnEnable()
		{
			_attemptsLabel.text = $"Attempts: {_attemptTracker.Attempts}";
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