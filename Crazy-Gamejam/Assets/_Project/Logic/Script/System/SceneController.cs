using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneController : MonoBehaviour
{
    // Refactor this script
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void RestartGame()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    private void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
