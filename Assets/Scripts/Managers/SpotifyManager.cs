using System;
using UnityEngine;
using UnityEngine.UI;

public class SpotifyManager : MonoBehaviour
{
    private AndroidJavaClass mainActivityClass;
    private AndroidJavaObject mainActivityObject;
    
    public Text currentPlayingText;

    void Start()
    {
        mainActivityClass = new AndroidJavaClass("com.unity3d.player.MainActivity");
        mainActivityObject = mainActivityClass.GetStatic<AndroidJavaObject>("instance");
        mainActivityObject.Call("PlayPlaylist");
    }
    
    private void Update()
    {
        UpdateCurrentPlaying();
    }

    public void Pause()
    {
        mainActivityObject.Call("Pause");
    }

    public void Resume()
    {
        mainActivityObject.Call("Resume");
    }

    public void SkipNext()
    {
        mainActivityObject.Call("SkipNext");
    }

    public void SkipPrevious()
    {
        mainActivityObject.Call("SkipPrevious");
    }

    public void ShuffleButtonOn()
    {
        mainActivityObject.Call("setRepeat", 2);
        mainActivityObject.Call("setShuffle", true);
    }

    public void ShuffleButtonOff()
    {
        mainActivityObject.Call("setRepeat", 2);
        mainActivityObject.Call("setShuffle", false);
    }
    
    public void RepeatButtonOn()
    {
        mainActivityObject.Call("setRepeat", 1);
        mainActivityObject.Call("setShuffle", false);
    }
    public void RepeatButtonOff()
    {
        mainActivityObject.Call("setRepeat", 2);
        mainActivityObject.Call("setShuffle", false);
    }

    public void UpdateCurrentPlaying()
    {
        currentPlayingText.text = mainActivityClass.GetStatic<string>("currentPlaying");
    }
}
