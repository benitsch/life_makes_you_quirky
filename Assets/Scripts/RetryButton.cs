using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
	private Button _retryButton;

    private void Awake()
    {
		_retryButton = GetComponent<Button>();
		_retryButton.onClick.AddListener(LoadFirstLevelScene);
	}

	void LoadFirstLevelScene()
	{
		SceneManager.LoadScene("Level0");
	}
}