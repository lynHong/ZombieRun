using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataManager : MonoBehaviour
{
    private List<int> heartRateData = new List<int>();

    // 事件响应方法
    public void OnHeartRateUpdated(int heartRate)
    {
        heartRateData.Add(heartRate);
        Debug.Log("Heart rate updated: " + heartRate);
    }

    // 保存心率数据到 JSON 文件的方法
    public void SaveHeartRateDataToJson()
    {
        string json = JsonUtility.ToJson(new HeartRateDataList(heartRateData));
        string path = Path.Combine(Application.persistentDataPath, "heartRateData.json");
        File.WriteAllText(path, json);
        Debug.Log("Heart rate data saved to " + path);
    }

    void Start()
    {
        FindObjectOfType<GameDataManager>();
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

