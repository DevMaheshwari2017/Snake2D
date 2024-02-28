using UnityEngine;

public static class SoundManager
{
    public enum Sounds
    {
        Background,
        SnakeMove,
        SnakeDie,
        SnakeEat,
        ButtonClicked,
        Powerup
    }
   public static void PlaySound(Sounds sounds)
    {
        GameObject soundgameobject = new GameObject("Sound");
        AudioSource audioSource = soundgameobject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioclip(sounds));
    }

    private static AudioClip GetAudioclip(Sounds sound)
    {
        foreach (GameAssests.SoundAudioClip soundAudioClip in GameAssests.instnace.soundsClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("sound " + sound + " not found");
        return null;
    }
}
