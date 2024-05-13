using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NowPlaying : MonoBehaviour
{

    public Text nowPlayingText;
    public UnityEngine.UI.Slider musicLength;


    void Update()
    {
        if (MusicManager.instance.CurrentTrackNumber() >= 0 && MusicManager.instance.NowPlaying() != null)
        {
            string timeText = SecondsToMS(MusicManager.instance.TimeInSeconds());
            string lengthText = SecondsToMS(MusicManager.instance.LengthInSeconds());

            nowPlayingText.text = "" + (MusicManager.instance.CurrentTrackNumber() + 1) + ". " +
                MusicManager.instance.NowPlaying().name + " (" + timeText + "/" + lengthText + ")";

            musicLength.value = MusicManager.instance.TimeInSeconds();
            musicLength.maxValue = MusicManager.instance.LengthInSeconds();
        }
        else
        {
            nowPlayingText.text = "-----------------";
            musicLength.value = 0;
            musicLength.maxValue = 1;
        }
    }


    string SecondsToMS(float seconds)
    {
        return string.Format("{0:D2}:{1:D2}", ((int)seconds) / 60, ((int)seconds) % 60);
    }
}
