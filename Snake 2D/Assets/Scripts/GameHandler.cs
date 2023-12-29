using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private SnakeMovement snake;
    [SerializeField]
    private player2Movment player2;
    private FoodSpawner levelGrid;
    private static GameHandler instance;
    private static int score;
    [SerializeField]
    private static int scoreValue = 10;
    private void Awake()
    {
        instance = this;
        score = 0;
    }
    private void Start()
    {
       
        Debug.Log("new snake");
        //setting foodspawner grid size same as background
        levelGrid = new FoodSpawner(19, 19);
        levelGrid.Setup(snake,player2);

        var scene = SceneManager.GetActiveScene();
        if(scene == SceneManager.GetSceneByBuildIndex(2))
        {
        player2.Setup(levelGrid);           
        }
        //getting the foodspawner ref
        snake.Setup(levelGrid);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += scoreValue;
    }

    public static void MinusScore()
    {
        if (score > 0)
        score -= scoreValue;
    }
    public static void GameOver()
    {
        GameReload.GameOverpanel();
    }

    public static void PauseGame()
    {
        PauseWindow.ShowPausescreeen();
        Time.timeScale = 0f;
    }

    public static void SetScoreValue(int s)
    {
        scoreValue = s;
    }
}
