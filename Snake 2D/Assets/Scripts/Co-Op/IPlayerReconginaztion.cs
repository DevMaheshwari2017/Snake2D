using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerReconginaztion
{
    void SetSnakeSpeed(float speed);
    bool SnakeIsAlive();
    int GetPlayerNumber();
}
