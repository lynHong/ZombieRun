using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XCharts;
using System.Linq;

public class ChartManager : MonoBehaviour
{
    [SerializeField] private LineChart m_chart;

    [SerializeField] private TextMeshProUGUI m_summaryText;
    private List<int> m_averageHeartRateList;

    private string m_gender;
    private string m_age;
    private string m_weight;
    private void Start()
    {
        SetupChart();
        m_gender = PlayerPrefs.GetString("Gender_SelectedOption", "DefaultGender");
        m_age = PlayerPrefs.GetString("Age_SelectedOption", "DefaultAge");
        m_weight = PlayerPrefs.GetString("Weight_SelectedOption", "DefaultWeight");
        UpdateSummaryText(m_averageHeartRateList.Count, CalculateTotalCalories(m_averageHeartRateList, m_gender, ParseAge(m_age), ParseWeight(m_weight)));
    }

    private void SetupChart()
    {
        m_chart.title.show = true;
        m_chart.title.text = "Heart Rate Graph";

        // Setup axes
        m_chart.xAxes[0].show = true;
        m_chart.xAxes[0].type = Axis.AxisType.Category;
        m_chart.xAxes[0].boundaryGap = true;
        m_chart.xAxes[0].splitNumber = 10;
        m_chart.yAxes[0].show = true;
        m_chart.yAxes[0].type = Axis.AxisType.Value;
        m_chart.yAxes[0].min = 0;
        m_chart.yAxes[0].max = 240;
        // Configure tooltip and legend
        m_chart.tooltip.show = true;
        m_chart.legend.show = false;

        // Clear existing data and add a new series
        m_chart.RemoveData();
        m_chart.AddSerie(SerieType.Line, "Heart Rate");


        for (int i = 0; i < m_averageHeartRateList.Count; i++)
        {
            m_chart.AddXAxisData("" + (i + 1));
            m_chart.AddData(0, m_averageHeartRateList[i]);
        }
    }

    public void UpdateChartWithData(List<int> heartRates)
    {
        m_averageHeartRateList = heartRates;
    }

    private double CalculateTotalCalories(List<int> heartRates, string gender, int age, double weight)
    {
        if (heartRates == null || heartRates.Count == 0)
        {
            return 0.0;
        }

        double averageHeartRate = heartRates.Average();
        int durationMinutes = heartRates.Count;

        double totalCalories = CalorieCalculator.CalculateCaloriesBurned((int)averageHeartRate, durationMinutes, weight, age, gender);

        return totalCalories;
    }


    public static class CalorieCalculator
    {
        public static double CalculateCaloriesBurned(int heartRate, int durationMinutes, double weightKg, int age, string gender)
        {

            double[] bmrFactors = gender == "Male"
                ? new double[] { -55.0969, 0.6309, 0.1988, 0.2017 }
                : new double[] { -20.4022, 0.4472, 0.1263, 0.074 };

            double totalCalories = ((bmrFactors[0] + (bmrFactors[1] * heartRate) + (bmrFactors[2] * weightKg) + (bmrFactors[3] * age)) / 4.184) * durationMinutes;

            return totalCalories;
        }
    }

    private void UpdateSummaryText(int totalMinutes, double totalCalories)
    {
        if (m_summaryText != null)
        {
            m_summaryText.text = $"Total Time: {totalMinutes} minutes\nTotal Calories: {totalCalories:F2} cal";
        }
        else
        {
            Debug.LogWarning("SummaryText is not assigned.");
        }
    }

    private int ParseAge(string ageOption)
    {
        string[] parts = ageOption.Split('-');
        if (parts.Length > 1 && int.TryParse(parts[1].Trim(), out int parsedAge))
        {
            return parsedAge;
        }
        return 70; // Default age if parsing fails
    }

    private double ParseWeight(string weightOption)
    {
        string[] parts = weightOption.Split('-');
        if (parts.Length > 1)
        {
            string secondPart = parts[1].Replace("KG", "").Trim();
            if (int.TryParse(secondPart, out int parsedWeight))
            {
                return parsedWeight;
            }
        }
        return 100; // Default weight if parsing fails
    }
}
