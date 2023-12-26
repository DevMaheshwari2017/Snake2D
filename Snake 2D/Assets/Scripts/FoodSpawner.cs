using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private Vector2Int foodGridPos;
    private GameObject foodGameobject;
    private int width;
    private int height;
    private SnakeMovement snake;

    //Giving a area where our food can spawn, setting thr grid size
    public FoodSpawner(int width, int height)
    {
        this.width = width;
        this.height = height;
        
    }

    public void Setup(SnakeMovement _snake)
    {
        this.snake = _snake;
        SpawnFood();
    }
    //Sapwing food in random width/height
    private void SpawnFood()
    {
        do
        {
            foodGridPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeBodyPositionList().IndexOf(foodGridPos) != -1); // checking if snake + body parts pos is same as food pos if yes genrate new food at other ramdom pos

        foodGameobject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameobject.GetComponent<SpriteRenderer>().sprite = GameAssests.instnace.foodSprite;
        foodGameobject.transform.position = new Vector3(foodGridPos.x, foodGridPos.y);
    }

    //Destroying food object after snake collision 
    public bool HasSnakeEatenFood(Vector2Int snakeGridPos)
    {
        if(snakeGridPos == foodGridPos)
        {
            Object.Destroy(foodGameobject);
            SpawnFood();
            Debug.Log("food spawned");
            GameHandler.AddScore();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridpos)
    {
        if(gridpos.x < 0)
        {
            gridpos.x = width;
        }
        if (gridpos.x > width)
        {
            gridpos.x = 0;
        }
        if (gridpos.y < 0)
        {
            gridpos.y = height;
        }
        if (gridpos.y > height)
        {
            gridpos.y = 0;
        }
        return gridpos;
    }
}
