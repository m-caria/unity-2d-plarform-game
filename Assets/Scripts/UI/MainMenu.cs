using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuFrame;
    public GameObject optionsFrame;
    public GameObject creditsFrame;
    public GameObject backButton;

    private void Awake()
    {
        mainMenuFrame.SetActive(true);
        optionsFrame.SetActive(false);
        creditsFrame.SetActive(false);
        backButton.SetActive(false);
    }

    public void Play() => SceneManager.LoadScene(1);

    public void SelectLevel() { }

    public void Options() 
    {
        mainMenuFrame.SetActive(false);
        optionsFrame.SetActive(true);
        backButton.SetActive(true);
    }

    public void Credits() 
    {
        mainMenuFrame.SetActive(false);
        creditsFrame.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void GoToHome() 
    {
        mainMenuFrame.SetActive(true);
        optionsFrame.SetActive(false);
        creditsFrame.SetActive(false);
        backButton.SetActive(false);
    }
}
