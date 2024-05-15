using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SettlementDataManager : MonoBehaviour
{
    public TextMeshProUGUI heartRateText; // 用于显示心率数据的 UI 文本

    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "heartRateData.json");
        ReadAndDisplayHeartRateData(path);
    }

    void ReadAndDisplayHeartRateData(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HeartRateDataList heartRateDataList = JsonUtility.FromJson<HeartRateDataList>(json);

            // 使用 StringBuilder 构建显示文本
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("Heart Rate Data:");

            foreach (int heartRate in heartRateDataList.heartRateData)
            {
                stringBuilder.AppendLine(heartRate.ToString() + " BPM");
            }

            // 将构建的文本设置到 TextMeshPro 组件
            heartRateText.text = stringBuilder.ToString();
        }
        else
        {
            heartRateText.text = "No heart rate data found.";
        }
    }
}

