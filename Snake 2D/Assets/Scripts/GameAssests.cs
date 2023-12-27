using System;
using System.Collections.Generic;
using UnityEngine;

public class GameAssests : MonoBehaviour
{
    public static GameAssests instnace;


    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite snakeBodySprite;

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
