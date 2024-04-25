using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Labyrinth.UI
{
	[RequireComponent(typeof(Button))]
	public class RestartButton : MonoBehaviour
	{
		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() =>
			{
				int buildIndex = SceneManager.GetActiveScene().buildIndex;
				SceneManager.LoadScene(buildIndex);
			});
		}
	}
}