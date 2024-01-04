using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FoodSpawner : MonoBehaviour
{
    private List<Food> foods;
    private int width;
    private int height;
    private SnakeMovement snake;
    private player2Movment player2;

    private void Awake()
    {
        player2 = player2.GetComponent<player2Movment>();
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
        var scene = SceneManager.GetActiveScene();
        do
        {
            foodGridPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeBodyPositionList().IndexOf(foodGridPos) != -1 || // checking if snake + body parts pos is same as food pos if yes genrate new food at other ramdom pos
        (scene.name == "Co-Op" && player2.GetFullSnakeBodyPositionList().IndexOf(foodGridPos) != -1) || // checking if the scene is Co-Op then food should be spawned upon player2 too
        foods.Any(food => food.GridPos == foodGridPos));

        GameObject foodGameobject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameobject.GetComponent<SpriteRenderer>().sprite = foodSprite;
        foodGameobject.transform.position = new Vector3(foodGridPos.x, foodGridPos.y);

        foods.Add(new Food { GridPos = foodGridPos, FoodGameObject = foodGameobject, FoodSprite = foodSprite, TypeOfFood = foodType });
    }

    //Destroying food object after snake collision 
    public bool HasSnakeEatenFood()
    {
        var scene = SceneManager.GetActiveScene();
        Food eatenFood = foods.FirstOrDefault(food => food.GridPos == snake.GetSnakeGridPosition() ||
                          (scene.name == "Co-Op" && food.GridPos == player2.GetSnakeGridPosition()));
        //Food p2_eatenFood = foods.FirstOrDefault(food => food.GridPos ==player2.GetSnakeGridPosition());
        if (eatenFood != null)
        {
                Object.Destroy(eatenFood.FoodGameObject);
                foods.Remove(eatenFood);
            if (eatenFood.GridPos == snake.GetSnakeGridPosition())
            {
                // Player 1 ate the food
                HandleFoodConsumptionPlayer1(eatenFood);
            }
            else if (scene.name == "Co-Op" && eatenFood.GridPos == player2.GetSnakeGridPosition())
            {
                // Player 2 ate the food
                HandleFoodConsumptionPlayer2(eatenFood);
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
    private void HandleFoodConsumptionPlayer1(Food eatenFood)
    {
        if (eatenFood.TypeOfFood == "gainer")
        {
            SoundManager.PlaySound(SoundManager.Sounds.SnakeEat);
            snake.IncreaseSnakeSize();
            GameHandler.AddToPlayer1Score();
        }
        else if (eatenFood.TypeOfFood == "burner")
        {
            SoundManager.PlaySound(SoundManager.Sounds.SnakeEat);
            snake.DecreaseSnakeSize();
            GameHandler.MinusPlayer1Score();
        }
    }

    private void HandleFoodConsumptionPlayer2(Food eatenFood)
    {
        if (eatenFood.TypeOfFood == "gainer")
        {
            SoundManager.PlaySound(SoundManager.Sounds.SnakeEat);
            player2.IncreaseSnakeSize();
            GameHandler.AddToPlayer2Score();
        }
        else if (eatenFood.TypeOfFood == "burner")
        {
            SoundManager.PlaySound(SoundManager.Sounds.SnakeEat);
            player2.DecreaseSnakeSize();
            GameHandler.MinusPlayer2Score();
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
