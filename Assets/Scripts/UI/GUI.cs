using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUI : MonoBehaviour
{
    [Header("Header Settings")]
    public GameObject liveImage;
    public Transform livesContainer;
    public TextMeshProUGUI fruitText;
    public TextMeshProUGUI timerText;
    public bool useTimer = true;
    public float levelCompleteTimerSeconds = 180.0F;

    [Header("GUI Frames")]
    public GameObject gameOverFrame;
    public GameObject pauseMenu;

    private float currentTime = 0.0F;
    private bool isGameOver = false;

    private void Awake()
    {
        gameOverFrame.SetActive(false);
        pauseMenu.SetActive(false);

        currentTime = levelCompleteTimerSeconds;

        if (!useTimer)
        {
            timerText.text = "";
        }
    }

    public void PrepareGUI(PlayerStats playerStats)
    {
        for (int i = 0; i < playerStats.Lives; i++)
        {
            Vector2 position = new(livesContainer.position.x + 40.0F * i, livesContainer.position.y - 8.0F);
            Instantiate(liveImage, position, Quaternion.identity, livesContainer);
        }

        fruitText.text = $"Fruits: x{playerStats.Fruits}";
    }

    public void RemoveLive()
    {
        Destroy(livesContainer.GetChild(livesContainer.childCount - 1).gameObject);
        if (livesContainer.childCount == 1)
            GameOver();
    }

    public void UpdateFruit(int quantity)
    {
        fruitText.text = $"Fruits: x{quantity}";
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverFrame.SetActive(true);
        GameManager.PauseGame();
    }

    private void Update()
    {
        if (useTimer && !IsTimerEnd())
        {
            currentTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            timerText.text = time.ToString(@"mm\:ss\:fff");
        }

        if (GameManager.IsGamePaused && !isGameOver)
            pauseMenu.SetActive(true);
        else pauseMenu.SetActive(false);
    }

    public bool IsTimerEnd() => currentTime <= 0.0F;
    public bool IsGameOver() => isGameOver;

    public void PlayAgain()
    {
        SceneManager.LoadScene(GameManager.GetCurrentScene());
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
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
}
