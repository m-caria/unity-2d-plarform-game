using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("GUI")]
    public GameObject guiGameObject;

    [Header("Game")]
    public static int currentScene = 1;

    public static bool IsGamePaused { get; private set; }

    private static GUI gui;

    private void Awake()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
        gui = guiGameObject.GetComponent<GUI>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public static void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
    }

    public static GUI GetGUI() => gui;
    public static int GetCurrentScene() => currentScene;
}
