using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject title;
    public GameObject pauseFrame;
    public GameObject optionsFrame;

    private void Start()
    {
        title.SetActive(true);
        pauseFrame.SetActive(true);
        optionsFrame.SetActive(false);
    }

    public void OnResume()
    {
        GameManager.ResumeGame();
    }

    public void OptionsMenu()
    {
        title.SetActive(false);
        pauseFrame.SetActive(false);
        optionsFrame.SetActive(true);
    }

    public void BackToPause()
    {
        title.SetActive(true);
        optionsFrame.SetActive(false);
        pauseFrame.SetActive(true);
    }

    public void OnQuit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnGoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
