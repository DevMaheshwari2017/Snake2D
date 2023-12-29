using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]
    SnakeMovement snake;
    [Header("PowerUps")]
    private bool isPowerUpPresent;
    [Header("Score PowerUp")]
    [SerializeField]
    private float timeofActivation = 10f;
    private float scorePowerTimer = 0;
    private bool scoreMultiplayer = false;
    [Header("Speed PowerUp")]
    [SerializeField]
    private float timeOfSpeedPowerActivation = 5f;
    private float speedPowerTimer = 0f;
    private bool isSpeedPowerActivated = false;
    [Header("Sheild PowerUp")]
    [SerializeField]
    private float timeOfSheildActivation;
    private float shieldPowerTimer = 0;
    private bool isSheildActivated = false;

    private IPlayerReconginaztion powerCollector;

    private void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }
    
    private void Update()
    {
        ScorePowerUp();
        SpeedPowerUp();
        ShieldPowerUp();
        
    }
    IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            if (!isPowerUpPresent)
            {
                int RandomPower = Random.Range(0, GameAssests.instnace.powerUps.Length);
                Vector3 Spawnpos;
                 do
                 {
                   Spawnpos = new Vector3(Random.Range(0, 19), Random.Range(0, 19), 0);   // checking whether any powerup is spawning upon the snake body or head ir yes then spaw power again until it doesn't spawn on the body/head
                 } while (snake.GetFullSnakeBodyPositionList().Any(pos => pos == new Vector2Int((int)Spawnpos.x, (int)Spawnpos.y)) || snake.GetSnakeGridPosition() == new Vector2Int((int)Spawnpos.x, (int)Spawnpos.y));
               
                GameObject newPowerUp = new GameObject("PowerUp");
                newPowerUp.tag = "PowerUp";
                newPowerUp.AddComponent<BoxCollider2D>().isTrigger = true;

                // Add the SpriteRenderer component
                SpriteRenderer renderer = newPowerUp.AddComponent<SpriteRenderer>();

                // Set the sprite of the SpriteRenderer to the randomly selected power-up
                renderer.sprite = GameAssests.instnace.powerUps[RandomPower];

                // Set the position of the new GameObject
                newPowerUp.transform.position = Spawnpos;
                isPowerUpPresent = true;
            }
            float waitTime = Random.Range(5f, 8f);
            yield return new WaitForSeconds(waitTime);
            Debug.Log("spawning powerUps");
        }
    }

    //private void PowerUpsTimer()
    //{    
    //    timer += Time.deltaTime;
    //}
    //public void PowerUp(bool canPowerActivate, float powerActivationTime, string whichPowerToActivate)
    //{
    //    if (canPowerActivate)
    //    {
    //        Debug.Log("power got activated");
    //        PowerUpsTimer();
    //        if (timer >= powerActivationTime)
    //        {
    //            canPowerActivate = false;
    //            timer = 0;
    //            switch (whichPowerToActivate)
    //            {
    //                case "ScorePowerUp":
    //                    ScorePowerUp(10);
    //                    Debug.Log("Score powerUp Deactivated"); break;
    //                case "SpeedPowerUp":
    //                    snake.SetSnakeSpeed(0.25f);
    //                    Debug.Log("Speed Power Deactivated"); break;
    //                case "SheildpowerUp": Debug.Log("Sheild powerUp Deactivated"); break;
    //            }
    //        }
        
    //      else
    //      {
    //        switch (whichPowerToActivate)
    //        {
    //            case "ScorePowerUp":
    //                ScorePowerUp(20);
    //                Debug.Log("Score powerUp Activated"); break;
    //            case "SpeedPowerUp":
    //                snake.SetSnakeSpeed(0.125f);
    //                Debug.Log("Speed Power Activated"); break;
    //            case "SheildpowerUp":
    //                snake.SnakeIsAlive();
    //                Debug.Log("Sheild powerUp Activated"); break;
    //        }
    //      }
    //    }
    //}
    private void ScorePowerUp()
    {
        if (scoreMultiplayer)
        {
            scorePowerTimer += Time.deltaTime;
            if (scorePowerTimer >= timeofActivation)
            {
                scoreMultiplayer = false;
                scorePowerTimer = 0;
                if (powerCollector.GetPlayerNumber() == 1)
                {
                    Player1ScorePowerUp(10);
                }
                else if (powerCollector.GetPlayerNumber() == 2)
                {
                    Player2ScorePowerUp(10);
                }
                Debug.Log("Score powerUp Deactivate");
            }
            else
            {
                if (powerCollector.GetPlayerNumber() == 1)
                {
                    Player1ScorePowerUp(20);
                }
                else if (powerCollector.GetPlayerNumber() == 2)
                {
                    Player2ScorePowerUp(20);
                }
                Debug.Log("Score powerUp Activate");
            }
        }
    } 
    private void SpeedPowerUp()
    {
        if (isSpeedPowerActivated)
        {
            speedPowerTimer += Time.deltaTime;
            if (speedPowerTimer >= timeOfSpeedPowerActivation)
            {
                Debug.Log("Speed Power Deactivated");
                isSpeedPowerActivated = false;
                powerCollector.SetSnakeSpeed(0.25f);
                speedPowerTimer = 0;
            }
            else
            {
                Debug.Log("Speed PowerUp Activated");
                powerCollector.SetSnakeSpeed(0.125f);
            }
        }
    }

    private void ShieldPowerUp()
    {
        if (isSheildActivated)
        {
            shieldPowerTimer += Time.deltaTime;
            if (shieldPowerTimer >= timeOfSheildActivation)
            {
                isSheildActivated = false;
                shieldPowerTimer = 0;
                Debug.Log("Sheild powerUp Deactivate");
            }
            else
            {
                powerCollector.SnakeIsAlive();
                Debug.Log("Sheild powerUp Activate");
            }
        }
    }
 
    //Getter
    public bool GetIsSheildPowerActivated()
    {
        return isSheildActivated;
    }
    //setter
    private void Player1ScorePowerUp(int s)
    {
        GameHandler.SetPlayer1ScoreValue(s);
    }     
    private void Player2ScorePowerUp(int s)
    {
        GameHandler.SetPlayer2ScoreValue(s);
    } 

    public void SetscoreMultiplayer(bool CanPowerActivate, IPlayerReconginaztion collector)
    {
        scoreMultiplayer = CanPowerActivate;
        powerCollector = collector;
    }
    public void SetSpeedPowerActivated(bool _isSpeedPowerActivated, IPlayerReconginaztion collector)
    {
        isSpeedPowerActivated = _isSpeedPowerActivated;
        powerCollector = collector;
    }
    public void SetSheildPowerActivated(bool _isSheildPowerActivated, IPlayerReconginaztion collector)
    {
        isSheildActivated = _isSheildPowerActivated;
        powerCollector = collector;
    }

    public void SetPowerPresentInTheGame(bool powerPresent)
    {
        isPowerUpPresent = powerPresent;
    }

 
}
