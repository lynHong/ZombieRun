using System;
using System.Collections.Generic;

[System.Serializable]
public class HeartRateDataList
{
    public List<MinuteHeartRate> heartRateData;

    public HeartRateDataList(List<MinuteHeartRate> data)
    {
        heartRateData = data;
    }
}