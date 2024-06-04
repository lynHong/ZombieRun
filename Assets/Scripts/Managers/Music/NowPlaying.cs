using UnityEngine;
using UnityEngine.UI;

public class NowPlaying : MonoBehaviour
{
    public Text nowPlayingText;
    public Slider musicLength;
    public SpinRecord spinRecord; // 引用 SpinRecord 脚本

    void Start()
    {
        // 检查 spinRecord 是否已被赋值
        if (spinRecord == null)
        {
            Debug.LogError("SpinRecord script is not assigned in the inspector.");
        }
    }

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

            // 控制唱片旋转状态
            if (MusicManager.instance.IsPlaying() && !spinRecord.IsPlaying)
            {
                spinRecord.StartPlaying();
            }
            else if (!MusicManager.instance.IsPlaying() && spinRecord.IsPlaying)
            {
                spinRecord.StopPlaying();
            }
        }
        else
        {
            nowPlayingText.text = "-----------------";
            musicLength.value = 0;
            musicLength.maxValue = 1;

            // 停止旋转
            if (spinRecord.IsPlaying)
            {
                spinRecord.StopPlaying();
            }
        }
    }

    string SecondsToMS(float seconds)
    {
        return string.Format("{0:D2}:{1:D2}", ((int)seconds) / 60, ((int)seconds) % 60);
    }
}
