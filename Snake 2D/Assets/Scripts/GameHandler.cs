using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private SnakeMovement snake;
    private FoodSpawner levelGrid;
    private static GameHandler instance;
    private static int score;

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
        levelGrid.Setup(snake);

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
        score += 10;
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
}
