using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    private void Update()
    {
        scoreText.text = "Score: " + GameHandler.GetScore().ToString();
    }
}
