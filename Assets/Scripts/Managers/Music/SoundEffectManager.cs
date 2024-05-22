using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource1; // Low heart rate audio
    [SerializeField] private AudioSource audioSource2; // High heart rate audio
    [SerializeField] private AudioClip[] zombieSounds; // Array to hold zombie sound clips

    private int m_heartRate;
    private bool m_hasReceivedFirstRate = false;
    [SerializeField] private ObjectEventSO m_zombieVoiceEvent;

    private int m_model;
    private int m_lowerBound; // Lower bound of the heart rate zone
    private int m_upperBound; // Upper bound of the heart rate zone

    void Start()
    {
        m_model = PlayerPrefs.GetInt("Model", 0);
        SetHeartRateBounds();
        LoadZombieSounds();
    }

    void Update()
    {
        if (m_hasReceivedFirstRate)
        {
            PlayAudioBasedOnRate();
        }
    }

    private void SetHeartRateBounds()
    {
        // Set the heart rate bounds based on the model
        switch (m_model)
        {
            case 1:
                m_lowerBound = 95;
                m_upperBound = 114;
                Debug.Log("Model 1 selected. Heart rate bounds set to 95-114.");
                break;
            case 2:
                m_lowerBound = 114;
                m_upperBound = 133;
                Debug.Log("Model 2 selected. Heart rate bounds set to 114-133.");
                break;
            case 3:
                m_lowerBound = 133;
                m_upperBound = 162;
                Debug.Log("Model 3 selected. Heart rate bounds set to 133-162.");
                break;
            default:
                m_lowerBound = 0;
                m_upperBound = 0; // Default or undefined model
                Debug.Log("No valid model selected. Heart rate bounds set to 0-0.");
                break;
        }
    }

    private void LoadZombieSounds()
    {
        // Load all zombie sounds from the Resources folder
        zombieSounds = Resources.LoadAll<AudioClip>("Sounds/Z1-Free");
    }

    private void PlayAudioBasedOnRate()
    {
        if (m_heartRate <= m_lowerBound)
        {
            if (!audioSource1.isPlaying)
            {
                audioSource1.clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
                audioSource1.Play();
                m_zombieVoiceEvent?.RaiseEvent(null, this);
                audioSource2.Stop(); // Ensure the second audio source is stopped
            }
        }
        else if (m_heartRate >= m_upperBound)
        {
            if (!audioSource2.isPlaying)
            {
                audioSource2.Play();
                audioSource1.Stop(); // Ensure the first audio source is stopped
            }
        }
        else
        {
            // If the heart rate is between the bounds, stop both audio sources
            if (audioSource1.isPlaying) audioSource1.Stop();
            if (audioSource2.isPlaying) audioSource2.Stop();
        }
    }
    public void OnHeartRateUpdatedEvent(int heartRate)
    {
        if (!m_hasReceivedFirstRate)
        {
            m_hasReceivedFirstRate = true;
        }
        m_heartRate = heartRate;
    }
}
