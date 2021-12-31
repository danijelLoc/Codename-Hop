using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject FailedUI;
    public GameObject CompleteUI;
    public static bool GameFailed = false;
    public static bool LevelComplete = false;

    private void Update()
    {
        if (!GameFailed && !LevelComplete)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        else if (GameFailed)
        {
            FailedUI.SetActive(true);
        }

        else if (LevelComplete)
        {
            CompleteUI.SetActive(true);
        }

    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        GameFailed = false;
        LevelComplete = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        GameFailed = false;
        LevelComplete = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameFailed = false;
        LevelComplete = false;
    }

}
