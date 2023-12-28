using System;
using System.Collections.Generic;
using UnityEngine;

public class GameAssests : MonoBehaviour
{
    public static GameAssests instnace;


    public Sprite snakeHeadSprite;
    public Sprite gainerFoodSprite;
    public Sprite burnerFoodSprite;
    public Sprite snakeBodySprite;
    public Sprite[] powerUps;

    public SoundAudioClip[] soundsClipArray;

    private void Awake()
    {
        instnace = this;
    }

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sounds sound;
        public AudioClip audioClip;
    }
}
