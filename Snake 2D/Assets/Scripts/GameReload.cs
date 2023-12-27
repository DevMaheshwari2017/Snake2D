using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReload : MonoBehaviour
{
    [SerializeField]
    private int mainLevel;
    [SerializeField]
    private GameObject gameover_Panel;

    private static GameReload instance;
    private void Awake()
    {
        instance = this;
        gameover_Panel.SetActive(false);
    }
    public void LoadSinglepLayerMainLevel()
   {
        SoundManager.PlaySound(SoundManager.Sounds.ButtonClicked);
        SceneManager.LoadScene(mainLevel);
   }

    private void ShowGameOverPanel()
    {
        gameover_Panel.SetActive(true);
    }

    public static void GameOverpanel()
    {
        instance.ShowGameOverPanel();
    }
}
