using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private Text player1ScoreText;
    [SerializeField]
    private Text player2ScoreText;

    private void Update()
    {
        player1ScoreText.text = "Score: " + GameHandler.GetPlayer1Score().ToString();

        var scene = SceneManager.GetActiveScene();
        if(scene.name == "Co-Op")
        {
        player2ScoreText.text = "Player 2 Score: " + GameHandler.GetPlayer2Score().ToString();
        player1ScoreText.text = "Player 1 Score: " + GameHandler.GetPlayer1Score().ToString();
        }
    }
}
