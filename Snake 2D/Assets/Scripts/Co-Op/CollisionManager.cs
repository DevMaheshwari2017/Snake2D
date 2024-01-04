using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionManager : MonoBehaviour
{
    [SerializeField]
    private SnakeMovement player1;
    [SerializeField]
    private player2Movment player2;
    [SerializeField]
    private GameObject gameover_Panel;
    [SerializeField]
    private GameObject player1_Won; 
    [SerializeField]
    private GameObject player2_Won;
    [SerializeField]
    private GameObject WonByScore;
    [SerializeField]
    private Text WonByScore_Text;
    [SerializeField]
    private GameObject draw;
    [SerializeField]
    private Text draw_Text;
    [SerializeField]
    private GameObject player1Controls;    
    [SerializeField]
    private GameObject player2Controls;

    private void Awake()
    {
        WonByScore.SetActive(false);
        gameover_Panel.SetActive(false);
        player1_Won.SetActive(false);
        player2_Won.SetActive(false);
    }
    private void Update()
    {
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        var scene = SceneManager.GetActiveScene();

        if (scene.name == "Co-Op")
        {
            List<Vector2Int> player1Positions = player1.GetFullSnakeBodyPositionList();
            List<Vector2Int> player2Positions = player2.GetFullSnakeBodyPositionList();
            bool player1Collision = player2Positions.Contains(player1.GetSnakeGridPosition());
            bool player2Collision = player1Positions.Contains(player2.GetSnakeGridPosition());

            if (player1Collision && player2Collision)
            {
                player1.SnakeIsDead();
                player2.SnakeIsDead();
                player1Controls.SetActive(false);
                player2Controls.SetActive(false);
                // Both players collided with each other
                Debug.LogWarning("Draw");
                if (GameHandler.GetPlayer1Score() == GameHandler.GetPlayer2Score())
                {
                    gameover_Panel.SetActive(true);
                    draw.SetActive(true);
                    draw_Text.text = " It's a Draw...";
                }

                else if (GameHandler.GetPlayer1Score() > GameHandler.GetPlayer2Score())
                {
                   int scoreDif = GameHandler.GetPlayer1Score() - GameHandler.GetPlayer2Score();
                   gameover_Panel.SetActive(true);
                   WonByScore.SetActive(true);
                   WonByScore_Text.text = "Player 1 won by " + scoreDif;
                }

                else if (GameHandler.GetPlayer2Score() > GameHandler.GetPlayer1Score())
                {
                   int scoreDif = GameHandler.GetPlayer2Score() - GameHandler.GetPlayer1Score();
                   gameover_Panel.SetActive(true);
                   WonByScore.SetActive(true);
                   WonByScore_Text.text = "Player 2 won by " + scoreDif;
                }
                
            }
            else if (player1Collision)
            {
                // Player 1 collided with Player 2's body, so Player 1 loses
                gameover_Panel.SetActive(true);
                player2_Won.SetActive(true);
                player2.SnakeIsDead();
                player1.SnakeIsDead();
                player1Controls.SetActive(false);
                player2Controls.SetActive(false);
                Debug.Log("Player 2 wins");
                // ...
            }
            else if (player2Collision)
            {
                // Player 2 collided with Player 1's body, so Player 2 loses
                gameover_Panel.SetActive(true);
                player1_Won.SetActive(true);
                player2.SnakeIsDead();
                player1.SnakeIsDead();
                player1Controls.SetActive(false);
                player2Controls.SetActive(false);
                Debug.Log("Player 1 wins");
                // ...
            }

            // Check for self-collisions
            // Player 1
            for (int i = 1; i < player1Positions.Count; i++)
            {
                if (player1.GetSnakeGridPosition() == player1Positions[i])
                {
                    Debug.Log("Player 1 collided with self");
                    // Handle player 1 self-collision
                    gameover_Panel.SetActive(true);
                    player2_Won.SetActive(true);
                    player2.SnakeIsDead();
                    player1.SnakeIsDead();
                    player1Controls.SetActive(false);
                    player2Controls.SetActive(false);
                    return;
                }
            }

            // Player 2
            for (int i = 1; i < player2Positions.Count; i++)
            {
                if (player2.GetSnakeGridPosition() == player2Positions[i])
                {
                    Debug.Log("Player 2 collided with self");
                    // Handle player 2 self-collision
                    gameover_Panel.SetActive(true);
                    player1_Won.SetActive(true);
                    player2.SnakeIsDead();
                    player1.SnakeIsDead();
                    player1Controls.SetActive(false);
                    player2Controls.SetActive(false);
                    return;
                }
            }
        }
    }
}
