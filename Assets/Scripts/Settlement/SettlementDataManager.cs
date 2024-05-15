using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettlementDataManager : MonoBehaviour
{
    [SerializeField] private IntListEventSO m_onTotalHeartRateDataLoaded;

    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "heartRateData.json");
        List<int> averageHeartRates = ReadHeartRateData(path);
        m_onTotalHeartRateDataLoaded.RaiseEvent(averageHeartRates, this);
    }

    private List<int> ReadHeartRateData(string path)
    {
        List<int> averageHeartRates = new List<int>();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HeartRateDataList heartRateDataList = JsonUtility.FromJson<HeartRateDataList>(json);
            if (heartRateDataList != null && heartRateDataList.heartRateData != null)
            {
                foreach (var minuteData in heartRateDataList.heartRateData)
                {
                    averageHeartRates.Add(minuteData.AverageHeartRate);
                }
            }
        }
        else
        {
            Debug.Log("No heart rate data found.");
        }
        return averageHeartRates;
    }
}
