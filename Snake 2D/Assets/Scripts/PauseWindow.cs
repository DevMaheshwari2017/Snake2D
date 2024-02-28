
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject pausedScreen;
    [SerializeField]
    private int mainMenu;

    private static PauseWindow instance;
    private void Awake()
    {
        instance = this;
        pausedScreen.SetActive(false);
    }

    public void ResumeGame()
    {
        SoundManager.PlaySound(SoundManager.Sounds.ButtonClicked);
        Time.timeScale = 1f;
        pausedScreen.SetActive(false);
    }

    public void QuitGame()
    {
        SoundManager.PlaySound(SoundManager.Sounds.ButtonClicked);
        Application.Quit();
    }

    public void ToMainMenu() 
    {
        SoundManager.PlaySound(SoundManager.Sounds.ButtonClicked);
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    private void show()
    {
        bool isPaused = !pausedScreen.activeSelf;
        Time.timeScale = isPaused ? 0f : 1f;
        pausedScreen.SetActive(isPaused);
    }

    public static void ShowPausescreeen()
    {
        instance.show();
    }
}
