using System;
using System.Collections.Generic;

[Serializable]
public class HeartRateDataList
{
    public List<int> heartRateData;

    public HeartRateDataList(List<int> heartRateData)
    {
        this.heartRateData = heartRateData;
    }
}
