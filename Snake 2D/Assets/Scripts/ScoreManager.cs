using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Update()
    {
        scoreText.text = "Score: " + GameHandler.GetScore().ToString();
    }
}
