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

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 10;
    }
}
