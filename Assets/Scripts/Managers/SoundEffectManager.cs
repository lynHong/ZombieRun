using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    public AudioSource audioSource1; // <80
    [SerializeField]
    public AudioSource audioSource2; // >160
    private int m_heartRate;


    void Update()
    {
        PlayAudioBasedOnRate();
    }
    // 根据心率值播放相应的音效
    private void PlayAudioBasedOnRate()
    {
        if (m_heartRate <= 80)
        {
            if (!audioSource1.isPlaying)
            {
                audioSource1.Play();
                audioSource2.Stop(); // 确保第二个音效停止播放
            }
        }
        else if (m_heartRate >= 160)
        {
            if (!audioSource2.isPlaying)
            {
                audioSource2.Play();
                audioSource1.Stop(); // 确保第一个音效停止播放
            }
        }
        else
        {
            // 如果心率在80到160之间，确保两个音效都不播放
            if (audioSource1.isPlaying) audioSource1.Stop();
            if (audioSource2.isPlaying) audioSource2.Stop();
        }
    }

    public void OnHeartRateUpdatedEvent(int heartRate)
    {
        m_heartRate = heartRate;
    }



}
