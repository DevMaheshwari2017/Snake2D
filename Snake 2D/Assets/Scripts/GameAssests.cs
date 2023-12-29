using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssests : MonoBehaviour
{
    public static GameAssests instnace;

    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite snakeBodySprite;
    private void Awake()
    {
        instnace = this;
    }

}
