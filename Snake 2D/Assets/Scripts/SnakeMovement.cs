using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeMovement : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    private enum State
    {
        Alive,
        Dead
    }
    [SerializeField]
    GameReload gameReload;
    private State state;
    private Direction grideMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    [SerializeField]
    private float gridMoveTimerMax;
    private FoodSpawner foodSpawner;
    private int snakeSize;
    //Store's the snake pos
    private List<SnakeMovePosition> snakeMovePosList;
    //How many body partt
    private List<SnakeBodyPart> snakeBodyPartList;

    private void Awake()
    {
        //default snake pos
        gridPosition = new Vector2Int(10, 10);

        gridMoveTimerMax = 0.25f;
        gridMoveTimer = gridMoveTimerMax;
        grideMoveDirection = Direction.Right;

        snakeSize = 0;
        snakeMovePosList = new List<SnakeMovePosition>();
        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Alive;

    }
    private void Update()
    {
        if (state == State.Dead)      
            return;
        
        MovementInput();
        GridMoevement();
    }

    //getting food spawner ref in game handler
    public void Setup(FoodSpawner _foodSpawner)
    {
        this.foodSpawner = _foodSpawner;
    }

    //Taking keyboard input 
    private void MovementInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))// S - W
        {
            //checking if the snake is not going in down dir only then we can move in up dir 
            if (grideMoveDirection != Direction.Down)
            {
                grideMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))// S - W
        {
            if (grideMoveDirection != Direction.Up)
            {
                grideMoveDirection = Direction.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) // A
        {
            if (grideMoveDirection != Direction.Left)
            {
                grideMoveDirection = Direction.Right;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // D
        {
            if (grideMoveDirection != Direction.Right)
            {
                grideMoveDirection = Direction.Left;
            }
        }
    }

    private void CheckingCollision()
    {
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
        {
           Vector2Int snakeBodyPartGridposition = snakeBodyPart.GetGridPosition();
            if (gridPosition == snakeBodyPartGridposition)
            {               
                state = State.Dead;
                GameHandler.GameOver();
                Debug.Log("game over");
            }
        }
    }
    private void GridMoevement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            //setting Movingtime back to 0 we can move again with time creating a loop
            gridMoveTimer -= gridMoveTimerMax;


            SnakeMovePosition previousSnakeMovepos = null;
            //checking if atleast have one position in the list 
            if (snakeMovePosList.Count >0)
            {
                // if so then we grab the position at the index 0 and that will become the previous position 
                previousSnakeMovepos = snakeMovePosList[0];
            }

            //Updating snake pos and direction as well as previous position  
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosition, grideMoveDirection,previousSnakeMovepos);
            //Instering new snake pos and dir
            snakeMovePosList.Insert(0, snakeMovePosition);

            //local var to tell which direction to move using enum 
            Vector2Int gridMoveDirectionVector;
            //Chaning the value of vector based on enum and moving in that direction 
            switch (grideMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            //Making snake move with time automatically 
            gridPosition += gridMoveDirectionVector;

            gridPosition = foodSpawner.ValidateGridPosition(gridPosition);
            CheckingCollision();

            //passing our snake pos and checking for collison with food
            bool snakeAteFood = foodSpawner.HasSnakeEatenFood(gridPosition);
            if (snakeAteFood)
            {
                //Snake has ate food inc body size
                snakeSize++;
                SpawingSnakeBody();
            }


            if (snakeMovePosList.Count >= snakeSize + 1)
            {
                snakeMovePosList.RemoveAt(snakeMovePosList.Count - 1);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);

            // rotates snake face 
            transform.eulerAngles = new Vector3(0, 0, SnakeRotattion(gridMoveDirectionVector) - 90);

            UpdateSnakeBodyPart();

        }
    }

    private void SpawingSnakeBody()
    {
        // creates a new instance of the class then add it to the list
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyPart()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePosList[i]);
        }
    }
    //Roating snake face in z dir 
    private float SnakeRotattion(Vector2Int dir)
    {
        float f = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (f < 0)
            f += 360;
        return f;
    }

    public Vector2Int getGridpos()
    {
        return gridPosition;
    }

    //returns the pos of snake head and body 
    public List<Vector2Int> GetFullSnakeBodyPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePosList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    //Instantiate new snake body part 
    private class SnakeBodyPart {

        private SnakeMovePosition snakeMovePosition;
        private Vector2Int gridPosition;
        private Transform transform;

        //constructor
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameobject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameobject.GetComponent<SpriteRenderer>().sprite = GameAssests.instnace.snakeBodySprite;
            //Changing the sortinglayer of evry next body part 
            snakeBodyGameobject.GetComponent<SpriteRenderer>().sortingOrder = - bodyIndex;
            transform = snakeBodyGameobject.transform;
        }
        public void SetSnakeMovePosition(SnakeMovePosition _snakeMovePosition)
        {
            this.snakeMovePosition = _snakeMovePosition;
            // moves the body part acc to snake grid pos;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x,snakeMovePosition.GetGridPosition().y);

            float angle;
            //Moving the snake body part with snake movement
            switch (snakeMovePosition.GetDirection()) {      //-90,90,180,0 cause ths sprite is horizontal 
                default:   

                case Direction.Up:  // curretnly going to Up
                    switch (snakeMovePosition.GetPreviousMovePosition())
                    {
                        default: angle = -90; break;
                        case Direction.Left: angle = -45; break; // previously was going down 
                        case Direction.Right: angle = 45; break; // previously was going Up
                    }
                    break; 

                case Direction.Down:  // curretnly going to Down
                    switch (snakeMovePosition.GetPreviousMovePosition())
                    {
                        default: angle = 90; break;
                        case Direction.Left: angle = 45; break; // previously was going down 
                        case Direction.Right: angle = -45; break; // previously was going Up
                    }

                    break; 

                case Direction.Left: // curretnly going to left 
                    switch (snakeMovePosition.GetPreviousMovePosition())
                    {
                        default: angle = 180; break;
                        case Direction.Down: angle = 180 + 45; break; // previously was going down 
                        case Direction.Up: angle = 180 - 45; break; // previously was going Up
                    }

                    break;

                case Direction.Right: // curretly going right 
                    switch (snakeMovePosition.GetPreviousMovePosition()) {
                        default: angle = 0; break;
                        case Direction.Down: angle = 0 - 45; break; // previously was going down 
                        case Direction.Up: angle = 0 + 45; break; // previously was going Up
                    }

                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }

    //Handels move pos of the snake
    private class SnakeMovePosition {

        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(Vector2Int gridPos, Direction dir, SnakeMovePosition _previousSnakeMovePosition)
        {
            this.previousSnakeMovePosition = _previousSnakeMovePosition;
            this.gridPosition = gridPos;
            this.direction = dir;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousMovePosition()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            return previousSnakeMovePosition.direction;
        }
    }


}
