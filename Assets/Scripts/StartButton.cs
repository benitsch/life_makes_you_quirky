using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
	private Button _startButton;

    private void Awake()
    {
		_startButton = GetComponent<Button>();
		_startButton.onClick.AddListener(LoadFirstLevelScene);
	}

	void LoadFirstLevelScene()
	{
		// TODO change name/scene from SampleScene to Level0
		SceneManager.LoadScene("Level0");
	}
}