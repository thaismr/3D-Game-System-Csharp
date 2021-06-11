using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour 
{

	void Start() 
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void LoadByIndex(int sceneIndex)
	{
		SceneManager.LoadSceneAsync (sceneIndex);
	}

	public void QuitGame()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
