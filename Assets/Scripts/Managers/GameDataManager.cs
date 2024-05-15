using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class GameDataManager : MonoBehaviour
{
    private Dictionary<int, List<int>> heartRateDataByMinute = new Dictionary<int, List<int>>();
    private DateTime startTime;

    void Start()
    {
        startTime = DateTime.Now;
    }

    public void OnHeartRateUpdated(int heartRate)
    {
        int minutesElapsed = (int)(DateTime.Now - startTime).TotalMinutes; // calculated minutes elapsed since start
        if (!heartRateDataByMinute.ContainsKey(minutesElapsed))
        {
            heartRateDataByMinute[minutesElapsed] = new List<int>();
        }
        heartRateDataByMinute[minutesElapsed].Add(heartRate);
    }

    public void SaveHeartRateDataToJson()
    {
        // calculate average heart rate per minute
        List<MinuteHeartRate> averageHeartRates = new List<MinuteHeartRate>();
        foreach (var minuteData in heartRateDataByMinute)
        {
            int averageRate = (int)Math.Round(minuteData.Value.Average());
            averageHeartRates.Add(new MinuteHeartRate { Minute = minuteData.Key, AverageHeartRate = averageRate });
        }

        // transfers data to JSON
        string json = JsonUtility.ToJson(new HeartRateDataList(averageHeartRates));
        string path = Path.Combine(Application.persistentDataPath, "heartRateData.json");
        File.WriteAllText(path, json);
    }

    void OnApplicationQuit()
    {
        SaveHeartRateDataToJson();
    }

    void OnDestroy()
    {
        SaveHeartRateDataToJson();
    }
}