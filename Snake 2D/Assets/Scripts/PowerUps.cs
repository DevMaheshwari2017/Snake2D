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

    private void ScorePowerUp()
    {
        if (scoreMultiplayer)
        {
            scorePowerTimer += Time.deltaTime;
            if (scorePowerTimer >= timeofActivation)
            {
                scoreMultiplayer = false;
                scorePowerTimer = 0;
                ScorePowerUp(10);
                Debug.Log("Score powerUp Deactivate");
            }
            else
            {
                ScorePowerUp(20);
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
                snake.SetSnakeSpeed(0.25f);
                speedPowerTimer = 0;
            }
            else
            {
                Debug.Log("Speed PowerUp Activated");
                snake.SetSnakeSpeed(0.125f);
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
                snake.SnakeIsAlive();
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
    private void ScorePowerUp(int s)
    {
        GameHandler.SetScoreValue(s);
    }
    public void SetscoreMultiplayer(bool CanPowerActivate)
    {
        scoreMultiplayer = CanPowerActivate;
    }
    public void SetSpeedPowerActivated(bool _isSpeedPowerActivated)
    {
        isSpeedPowerActivated = _isSpeedPowerActivated;
    }
    public void SetSheildPowerActivated(bool _isSheildPowerActivated)
    {
        isSheildActivated = _isSheildPowerActivated;
    }

    public void SetPowerPresentInTheGame(bool powerPresent)
    {
        isPowerUpPresent = powerPresent;
    }

 
}
