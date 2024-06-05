using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using XCharts;

public class ChartManager : MonoBehaviour
{
    [SerializeField] private LineChart chart;  
    [SerializeField] private TextMeshProUGUI summaryText;  

    private List<int> m_averageHeartRateList;

    private string gender;
    private int age;
    private double weight;

    private void Start()
    {
        // dummy data
        m_averageHeartRateList = new List<int> { 60, 75, 80, 120, 90, 85, 100, 110, 95, 80 };
        
        gender = PlayerPrefs.GetString("Gender_SelectedOption", "DefaultGender");
        age = ParseAge(PlayerPrefs.GetString("Age_SelectedOption", "DefaultAge"));
        weight = ParseWeight(PlayerPrefs.GetString("Weight_SelectedOption", "DefaultWeight")); 
        
        // Debug.Log("Weight: " + weight + " KG");
        // Debug.Log("Age: " + age);
        // Debug.Log("Gender: " + gender);

        SetupChart();

        double totalCalories = CalculateTotalCalories(m_averageHeartRateList, gender, age, weight);
        // Debug.Log("Total Calories Burned: " + totalCalories);

        UpdateSummaryText(m_averageHeartRateList.Count, totalCalories);
    }

    private void SetupChart()
    {
        chart.title.show = true;
        chart.title.text = "Statistic";

        chart.xAxes[0].show = true;
        chart.xAxes[0].type = Axis.AxisType.Category;
        chart.xAxes[0].boundaryGap = true;
        chart.xAxes[0].splitNumber = 10;
        chart.yAxes[0].show = true;
        chart.yAxes[0].type = Axis.AxisType.Value;
        chart.yAxes[0].min = 0; 
        chart.yAxes[0].max = 240; 

        chart.tooltip.show = true;
        chart.legend.show = false;

        chart.RemoveData();
        chart.AddSerie(SerieType.Line, "Heart Rate");

        if (m_averageHeartRateList != null)
        {
            for (int i = 0; i < m_averageHeartRateList.Count; i++)
            {
                chart.AddXAxisData("" + (i + 1));
                chart.AddData(0, m_averageHeartRateList[i]);
            }
        }
    }

    public void UpdateChartWithData(List<int> heartRates)
    {
        m_averageHeartRateList = heartRates;
        SetupChart();

        double totalCalories = CalculateTotalCalories(m_averageHeartRateList, gender, age, weight);

        UpdateSummaryText(m_averageHeartRateList.Count, totalCalories);
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

    private void UpdateSummaryText(int totalMinutes, double totalCalories)
    {
        if (summaryText != null)
        {
            summaryText.text = $"Total Time: {totalMinutes} minutes\nTotal Calories: {totalCalories:F2} cal";
        }
        else
        {
            Debug.LogWarning("SummaryText is not assigned.");
        }
    }
}

public static class CalorieCalculator
{
    public static double CalculateCaloriesBurned(int heartRate, int durationMinutes, double weightKg, int age, string gender)
    {

        double[] bmrFactors = gender == "Male" 
            ? new double[] {-55.0969, 0.6309, 0.1988, 0.2017} 
            : new double[] {-20.4022, 0.4472, 0.1263, 0.074};

        double totalCalories = ((bmrFactors[0] + (bmrFactors[1] * heartRate) + (bmrFactors[2] * weightKg) + (bmrFactors[3] * age)) / 4.184) * durationMinutes;

        return totalCalories;
    }
}
