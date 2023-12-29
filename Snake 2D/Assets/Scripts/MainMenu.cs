using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private int coOp_Level;
    [SerializeField]
    private int mainLevel;

    public void LoadCoOpLevel()
    {
        SoundManager.PlaySound(SoundManager.Sounds.ButtonClicked);
        SceneManager.LoadScene(coOp_Level);
    }

    public void QuitGame()
    {
        SoundManager.PlaySound(SoundManager.Sounds.ButtonClicked);
        Application.Quit();
        Debug.Log("Game QUit");
    }

    public void LoadMainLevel()
    {
        SoundManager.PlaySound(SoundManager.Sounds.ButtonClicked);
        SceneManager.LoadScene(mainLevel);
        Debug.Log("Loading main level" + mainLevel);
    }
}
