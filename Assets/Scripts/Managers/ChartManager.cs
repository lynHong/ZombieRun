using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XCharts;

public class ChartManager : MonoBehaviour
{
    [SerializeField] private LineChart chart;  // Assume this is already attached to the GameObject

    private List<int> m_averageHeartRateList;


    private void Start()
    {
        SetupChart();
    }

    private void SetupChart()
    {
        // Set the size of the chart (assumed to be set via Inspector or elsewhere in code)
        // chart.SetSize(580, 300);

        // Configure title
        chart.title.show = true;
        chart.title.text = "Heart Rate Graph";

        // Setup axes
        chart.xAxes[0].show = true;
        chart.xAxes[0].type = Axis.AxisType.Category;
        chart.xAxes[0].boundaryGap = true;
        chart.xAxes[0].splitNumber = 10;
        chart.yAxes[0].show = true;
        chart.yAxes[0].type = Axis.AxisType.Value;
        chart.yAxes[0].min = 0; // Setting the minimum value of y-axis
        chart.yAxes[0].max = 240; // Setting the maximum value of y-axis

        // Configure tooltip and legend
        chart.tooltip.show = true;
        chart.legend.show = false;

        // Clear existing data and add a new series
        chart.RemoveData();
        chart.AddSerie(SerieType.Line, "Heart Rate");


        for (int i = 0; i < m_averageHeartRateList.Count; i++)
        {
            chart.AddXAxisData("" + (i + 1));
            chart.AddData(0, m_averageHeartRateList[i]);
        }
    }

    public void UpdateChartWithData(List<int> heartRates)
    {
        m_averageHeartRateList = heartRates;
    }
}
