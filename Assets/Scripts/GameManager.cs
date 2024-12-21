using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("GUI")]
    public GameObject guiGameObject;

    [Header("Game")]
    public static int currentScene = 1;
    public static float levelCompleteTimerSeconds = 180.0F;

    public static bool IsGamePaused { get; private set; }
    public static bool IsLevelStarted { get; private set; }

    private static GUI gui;

    private void Awake()
    {
        IsGamePaused = false;
        IsLevelStarted = false;
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

    public static void StartLevel() => IsLevelStarted = true;
    public static void EndLevel() => IsLevelStarted = false;

    public static GUI GetGUI() => gui;
    public static int GetCurrentScene() => currentScene;
}
