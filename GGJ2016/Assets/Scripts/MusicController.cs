using UnityEngine;
using System.Collections;

public enum MusicType
{
    Title, 
    WayDown,
    WayUp
}
public class MusicController : MonoBehaviour
{

    public static MusicController controller;
    public AudioClip titleMusic, backgroundMusicDown, backgroundMusicUp;
    public AudioClip gettingWaterMusic, loseWaterMusic;

    AudioSource audioSource;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("Level loaded!  " + level);

        switch (level)
        {
            case 0: //title
                audioSource.clip = titleMusic;
                audioSource.Play();
                break;
            //case 2: //main
            //    audioSource.clip = backgroundMusicDown;
            //    audioSource.Play();
            //    break;
            case 3: //gameover
                audioSource.clip = titleMusic;
                audioSource.Play();
                break;
        }
    }

    public void PlaySong(MusicType type)
    {
        switch (type)
        {
            default:
            case MusicType.Title:
                audioSource.clip = titleMusic;
                break;
            case MusicType.WayDown:
                audioSource.clip = backgroundMusicDown;
                break;
            case MusicType.WayUp:
                audioSource.clip = backgroundMusicUp;
                break;
        }
        audioSource.Play();
    }
}
