using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource1; // <80
    [SerializeField] public AudioSource audioSource2; // >160
    private int m_heartRate;
    private bool hasReceivedFirstRate = false;

    [SerializeField] private ObjectEventSO m_zombieVoiceEvent;

    void Update()
    {
        if (hasReceivedFirstRate)
        {
            PlayAudioBasedOnRate();
        }
    }

    private void PlayAudioBasedOnRate()
    {
        if (m_heartRate <= 80)
        {
            if (!audioSource1.isPlaying)
            {
                audioSource1.Play();
                m_zombieVoiceEvent?.RaiseEvent(null, this);
                audioSource2.Stop(); // make sure the second audio source is stopped
            }
        }
        else if (m_heartRate >= 160)
        {
            if (!audioSource2.isPlaying)
            {
                audioSource2.Play();
                audioSource1.Stop(); // make sure the first audio source is stopped
            }
        }
        else
        {
            // if the heart rate is between 80 and 160, make sure both audio sources are stopped
            if (audioSource1.isPlaying) audioSource1.Stop();
            if (audioSource2.isPlaying) audioSource2.Stop();
        }
    }

    public void OnHeartRateUpdatedEvent(int heartRate)
    {
        if (!hasReceivedFirstRate)
        {
            hasReceivedFirstRate = true;
        }
        m_heartRate = heartRate;
    }
}
