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
    private static int player1Score;
    private static int player2Score;
    [SerializeField]
    private static int player1ScoreValue = 10;   
    [SerializeField]
    private static int player2ScoreValue = 10;
    private void Awake()
    {
        instance = this;
        player1Score = 0;
        player2Score = 0;
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

    public static void AddToPlayer1Score()
    {
        player1Score += player1ScoreValue;
    }

    public static void MinusPlayer1Score()
    {
        if (player1Score > 0)
        player1Score -= player1ScoreValue;
    } 

    public static void AddToPlayer2Score()
    {
        player2Score += player2ScoreValue;
    }

    public static void MinusPlayer2Score()
    {
        if (player2Score > 0)
        player2Score -= player2ScoreValue;
    }
    public static void GameOver()
    {
        GameReload.GameOverpanel();
    }

    public static void PauseGame()
    {
        PauseWindow.ShowPausescreeen(); 
    }

    //Getter
    public static int GetPlayer2Score()
    {
        return player2Score;
    }
    public static int GetPlayer1Score()
    {
        return player1Score;
    }
    //Setter
    public static void SetPlayer1ScoreValue(int s)
    {
        player1ScoreValue = s;
    } 
    public static void SetPlayer2ScoreValue(int s)
    {
        player2ScoreValue = s;
    }
}
