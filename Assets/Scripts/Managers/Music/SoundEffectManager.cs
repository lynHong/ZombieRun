using TMPro;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource1; // Low heart rate audio
    [SerializeField] private AudioSource audioSource2; // High heart rate audio
    [SerializeField] private AudioClip[] zombieSounds; // Array to hold zombie sound clips

    [SerializeField] private TextMeshProUGUI m_hint; // TextMeshProUGUI to display heart rate hint

    private int m_heartRate;
    private bool m_hasReceivedFirstRate = false;
    [SerializeField] private ObjectEventSO m_zombieVoiceEvent;

    private int m_model;
    private HeartRateBounds m_lightBounds;
    private HeartRateBounds m_moderateBounds;
    private HeartRateBounds m_intenseBounds;

    private string m_genderOption;
    private string m_ageOption;
    private string m_weightOption;

    void Start()
    {
        m_model = PlayerPrefs.GetInt("Model", 0);
        m_genderOption = PlayerPrefs.GetString("Gender_SelectedOption", "DefaultGender");
        m_ageOption = PlayerPrefs.GetString("Age_SelectedOption", "DefaultAge");
        m_weightOption = PlayerPrefs.GetString("Weight_SelectedOption", "DefaultWeight");



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
        // Define heart rate bounds based on gender, age, and weight
        m_lightBounds = CalculateHeartRateBounds(m_genderOption, m_ageOption, m_weightOption, "light");
        m_moderateBounds = CalculateHeartRateBounds(m_genderOption, m_ageOption, m_weightOption, "moderate");
        m_intenseBounds = CalculateHeartRateBounds(m_genderOption, m_ageOption, m_weightOption, "intense");

        HeartRateBounds selectedBounds;
        switch (m_model)
        {
            case 1:
                selectedBounds = m_lightBounds;
                break;
            case 2:
                selectedBounds = m_moderateBounds;
                break;
            case 3:
                selectedBounds = m_intenseBounds;
                break;
            default:
                selectedBounds = new HeartRateBounds(0, 0);
                Debug.Log("No valid model selected. Heart rate bounds set to 0-0.");
                break;
        }

        m_hint.text = $"Keep your heart rate between {selectedBounds.LowerBound} and {selectedBounds.UpperBound}";
    }

    private HeartRateBounds CalculateHeartRateBounds(string gender, string age, string weight, string intensity)
    {
        // Define example heart rate bounds logic based on provided parameters
        int baseLower = 0;
        int baseUpper = 0;

        // Adjust baseLower and baseUpper based on gender, age, weight, and intensity
        // This is a placeholder. Replace with actual logic based on medical or fitness guidelines.
        if (intensity == "light")
        {
            baseLower = 95;
            baseUpper = 133;
        }
        else if (intensity == "moderate")
        {
            baseLower = 133;
            baseUpper = 162;
        }
        else if (intensity == "intense")
        {
            baseLower = 162;
            baseUpper = 181;
        }

        // Further adjust bounds based on gender, age, and weight
        if (gender == "Male")
        {
            baseLower += 5;
            baseUpper += 5;
        }
        if (age == "0-10")
        {
            baseLower -= 10;
            baseUpper -= 10;
        }
        else if (age == "60 and above")
        {
            baseLower -= 5;
            baseUpper -= 5;
        }
        if (weight == "90 and above")
        {
            baseLower -= 5;
            baseUpper -= 5;
        }

        return new HeartRateBounds(baseLower, baseUpper);
    }

    private void LoadZombieSounds()
    {
        // Load all zombie sounds from the Resources folder
        zombieSounds = Resources.LoadAll<AudioClip>("Sounds/Z1-Free");
    }

    private void PlayAudioBasedOnRate()
    {
        HeartRateBounds bounds;
        switch (m_model)
        {
            case 1:
                bounds = m_lightBounds;
                break;
            case 2:
                bounds = m_moderateBounds;
                break;
            case 3:
                bounds = m_intenseBounds;
                break;
            default:
                bounds = new HeartRateBounds(0, 0);
                Debug.Log("No valid model selected. Heart rate bounds set to 0-0.");
                break;
        }

        if (m_heartRate <= bounds.LowerBound)
        {
            if (!audioSource1.isPlaying)
            {
                audioSource1.clip = zombieSounds[Random.Range(0, zombieSounds.Length)];
                audioSource1.Play();
                m_zombieVoiceEvent?.RaiseEvent(null, this);
                audioSource2.Stop(); // Ensure the second audio source is stopped
            }
        }
        else if (m_heartRate >= bounds.UpperBound)
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

public class HeartRateBounds
{
    public int LowerBound { get; set; }
    public int UpperBound { get; set; }

    public HeartRateBounds(int lower, int upper)
    {
        LowerBound = lower;
        UpperBound = upper;
    }
}
