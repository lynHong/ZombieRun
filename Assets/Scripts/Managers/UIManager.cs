using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI m_heartRate;

    public void OnHeartRateUpdatedEvent(int heartRate)
    {

        m_heartRate.text = heartRate.ToString();
    }
}
