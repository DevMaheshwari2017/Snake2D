using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodSpawner : MonoBehaviour
{
    private List<Food> foods;
    private int width;
    private int height;
    private SnakeMovement snake;
    private player2Movment player2;

    private void Awake()
    {
        //player2 = player2.GetComponent<player2Movment>();
    }
    //Giving a area where our food can spawn, setting thr grid size
    public FoodSpawner(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.foods = new List<Food>();
    }

    public void Setup(SnakeMovement _snake, player2Movment _player2)
    {
        this.snake = _snake;
        this.player2 = _player2;
        SpawnFood(GameAssests.instnace.gainerFoodSprite, "gainer");
        SpawnFood(GameAssests.instnace.burnerFoodSprite, "burner");
    }
    //Sapwing food in random width/height
    private void SpawnFood(Sprite foodSprite, string foodType)
    {
        Vector2Int foodGridPos;

        do
        {
            foodGridPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeBodyPositionList().IndexOf(foodGridPos) != -1); // checking if snake + body parts pos is same as food pos if yes genrate new food at other ramdom pos

        GameObject foodGameobject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameobject.GetComponent<SpriteRenderer>().sprite = foodSprite;
        foodGameobject.transform.position = new Vector3(foodGridPos.x, foodGridPos.y);

        foods.Add(new Food { GridPos = foodGridPos, FoodGameObject = foodGameobject, FoodSprite = foodSprite, TypeOfFood = foodType });
    }

    //Destroying food object after snake collision 
    public bool HasSnakeEatenFood()
    {
        Food eatenFood = foods.FirstOrDefault(food => food.GridPos == snake.GetSnakeGridPosition());
        //Food p2_eatenFood = foods.FirstOrDefault(food => food.GridPos ==player2.GetSnakeGridPosition());
        if (eatenFood != null)
        {
                Object.Destroy(eatenFood.FoodGameObject);
                foods.Remove(eatenFood);
            if (eatenFood.TypeOfFood == "gainer")
            {
                SoundManager.PlaySound(SoundManager.Sounds.SnakeEat);
                //snake.IncreaseSnakeSize();
                snake.IncreaseSnakeSize();
                GameHandler.AddScore();
            }

            else if (eatenFood.TypeOfFood == "burner")
            {
                SoundManager.PlaySound(SoundManager.Sounds.SnakeEat);
                snake.DecreaseSnakeSize();
                GameHandler.MinusScore();
            }
                Debug.Log("food spawned");
            SpawnFood(eatenFood.FoodSprite, eatenFood.TypeOfFood);
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

    public class Food
    {
        public Vector2Int GridPos { get; set; }
        public GameObject FoodGameObject { get; set; }
        public Sprite FoodSprite { get; set; }
        public string TypeOfFood { get; set; }
    }
}
