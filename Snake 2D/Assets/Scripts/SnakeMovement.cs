using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SnakeMovement : MonoBehaviour, IPlayerReconginaztion
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
    private float inputDelay;
    [SerializeField]
    private int mainScene;

    [Header("ScriptsRefrences")]
    [SerializeField]
    GameReload gameReload;
    private State state;
    private FoodSpawner foodSpawner;
    [SerializeField]
    private PowerUps powerUps;
    [SerializeField]
    private player2Movment player2;

    

    [Header("SnakeMovement&Size")]
    private Direction grideMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    //basically speed with respect to time 
    [SerializeField]
    private float gridMoveTimerMax;
    private int snakeSize;
    //Store's the snake pos
    private List<SnakeMovePosition> snakeMovePosList;
    //How many body partt
    private List<SnakeBodyPart> snakeBodyPartList;
    private bool isProcessingInput = false;

    [Header("Co-Op")]
    [SerializeField]
    private int CoOp_scene;

    [Header("For WebGl")]
    [SerializeField]
    private GameObject movementKeys;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PowerUp"))
        {
            Sprite powerUpSprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;
            if(powerUpSprite == GameAssests.instnace.powerUps[0])
            {
                Debug.Log("Score_PowerUp collected");
                SoundManager.PlaySound(SoundManager.Sounds.Powerup);
                Destroy(other.gameObject);
                powerUps.SetscoreMultiplayer(true,this);
                powerUps.SetPowerPresentInTheGame(false);
                
            }
            if(powerUpSprite == GameAssests.instnace.powerUps[1])
            {
                Debug.Log("Sheild_PowerUp collected");
                SoundManager.PlaySound(SoundManager.Sounds.Powerup);
                Destroy(other.gameObject);
                powerUps.SetSheildPowerActivated(true,this);
                powerUps.SetPowerPresentInTheGame(false);

            }
            if(powerUpSprite == GameAssests.instnace.powerUps[2])
            {
                Debug.Log("Speed_PowerUp collected");
                SoundManager.PlaySound(SoundManager.Sounds.Powerup);
                Destroy(other.gameObject);
                powerUps.SetSpeedPowerActivated(true,this);
                powerUps.SetPowerPresentInTheGame(false);
            }
        }
    }
    //Taking keyboard input For the singlePlayer game 
    private void MovementInput()
    {
        var scene = SceneManager.GetActiveScene();
        if (isProcessingInput)
            return;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if(scene == SceneManager.GetSceneByBuildIndex(CoOp_scene))
            {
            movementKeys.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && grideMoveDirection != Direction.Down)
            {
                grideMoveDirection = Direction.Up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && grideMoveDirection != Direction.Up)
            {
                grideMoveDirection = Direction.Down;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && grideMoveDirection != Direction.Right)
            {
                grideMoveDirection = Direction.Left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && grideMoveDirection != Direction.Left)
            {
                grideMoveDirection = Direction.Right;
            }
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (scene == SceneManager.GetSceneByBuildIndex(CoOp_scene))
            {
                movementKeys.SetActive(true);
            }
            else if (scene == SceneManager.GetSceneByBuildIndex(mainScene))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        if (direction.x > 0 && grideMoveDirection != Direction.Left)
                        {
                            grideMoveDirection = Direction.Right;
                        }
                        else if (direction.x < 0 && grideMoveDirection != Direction.Right)
                        {
                            grideMoveDirection = Direction.Left;
                        }
                    }
                    else
                    {
                        if (direction.y > 0 && grideMoveDirection != Direction.Down)
                        {
                            grideMoveDirection = Direction.Up;
                        }
                        else if (direction.y < 0 && grideMoveDirection != Direction.Up)
                        {
                            grideMoveDirection = Direction.Down;
                        }
                    }

                }
            }
        }       
                StartInputDelay();
    }

    // For Co-Op using buttons in the Co-Op instead of mouse or touch input for the WebGl 
    public void MoveUp()
    {
        if (isProcessingInput)
            return;

        if (grideMoveDirection != Direction.Down)
        {
            grideMoveDirection = Direction.Up;
            StartInputDelay();
        }
    }

    public void MoveDown()
    {
        if (isProcessingInput)
            return;

        if (grideMoveDirection != Direction.Up)
        {
            grideMoveDirection = Direction.Down;
            StartInputDelay();
        }
    }

    public void MoveLeft()
    {
        if (isProcessingInput)
            return;

        if (grideMoveDirection != Direction.Right)
        {
            grideMoveDirection = Direction.Left;
            StartInputDelay();
        }
    }

    public void MoveRight()
    {
        if (isProcessingInput)
            return;

        if (grideMoveDirection != Direction.Left)
        {
            grideMoveDirection = Direction.Right;
            StartInputDelay();
        }
    }
    private void StartInputDelay()
    {
        isProcessingInput = true;
        Invoke("StopInputDelay", inputDelay); // the delay so player can't continously press keys and change dierction 
    }

    private void StopInputDelay()
    {
        isProcessingInput = false;
    }
    private void CheckingCollision()
    {
        var scene = SceneManager.GetActiveScene();
        List<Vector2Int> Player1FullPosition = GetFullSnakeBodyPositionList();
        if (scene.name == "main")
        {
            // Check for collision with player1's own body
            for (int i = 1; i < Player1FullPosition.Count; i++) // Start from index 1 to exclude the head
            {
                if (gridPosition == Player1FullPosition[i])
                {
                    if (powerUps.GetIsSheildPowerActivated() == false)
                    {
                        SoundManager.PlaySound(SoundManager.Sounds.SnakeDie);
                        state = State.Dead;
                        GameHandler.GameOver();
                        Debug.Log("game over");

                    }
                }
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

            SoundManager.PlaySound(SoundManager.Sounds.SnakeMove);

            SnakeMovePosition previousSnakeMovepos = null;
            //checking if atleast have one position in the list 
            if (snakeMovePosList.Count > 0)
            {
                // if so then we grab the position at the index 0 and that will become the previous position 
                previousSnakeMovepos = snakeMovePosList[0];
            }

            //Updating snake pos and direction as well as previous position  
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosition, grideMoveDirection, previousSnakeMovepos);
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

            foodSpawner.HasSnakeEatenFood();
            //Increase_DecreaseSnakeSize();

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
    public void IncreaseSnakeSize()
    {
            snakeSize++;
            SpawingSnakeBody();
    } 
    public void DecreaseSnakeSize()
    {

        if (snakeSize > 0) // Ensure snake size is greater than 0 before decreasing
        {
            snakeSize--;
            // Getting the last body part
            SnakeBodyPart lastBodyPart = snakeBodyPartList[snakeBodyPartList.Count - 1];
            // Removeing it from the list
            snakeBodyPartList.RemoveAt(snakeBodyPartList.Count - 1);
            // Destroying the corresponding GameObject
            Destroy(lastBodyPart.GetSnakeTransform().gameObject);
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

    //Getter
    public Vector2Int GetSnakeGridPosition()
    {
        return gridPosition;
    }
    public float GetSnakeSpeed()
    {
        return gridMoveTimerMax;
    }
    public int GetPlayerNumber()
    {
        return 1;
    }
    //setter
    public void SetSnakeSpeed(float _speed)
    {
        gridMoveTimerMax = _speed;
    }
    public void SnakeIsAlive()
    {
        state = State.Alive;
    } public void SnakeIsDead()
    {
        state = State.Dead;
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

        public Transform GetSnakeTransform()
        {
            return transform;
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
