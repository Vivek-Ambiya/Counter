using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CounterManager : MonoBehaviour
{
    public TextMeshProUGUI totalCountText;
    public TextMeshProUGUI dailyCountText;
    public Button dailyCountButton;

    private int totalCount;
    private int dailyCount;

    private const string TOTAL_KEY = "TotalCount";
    private const string DAILY_KEY = "DailyCount";
    private const string RESET_TIME_KEY = "LastResetTime";

    void Start()
    {
        LoadData();
        Check24HourReset();
        UpdateUI();

        // Button listener
        dailyCountButton.onClick.AddListener(OnCountButtonClick);
    }

    public void OnCountButtonClick()
    {
        // Start timer on first click
        if (!PlayerPrefs.HasKey(RESET_TIME_KEY))
        {
            SaveResetTime();
        }

        Check24HourReset();

        dailyCount++;

        SaveData();
        UpdateUI();
    }

    void Check24HourReset()
    {
        if (!PlayerPrefs.HasKey(RESET_TIME_KEY))
            return;

        DateTime lastReset =
            DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString(RESET_TIME_KEY)));

        TimeSpan timePassed = DateTime.Now - lastReset;

        if (timePassed.TotalHours >= 24)
        {
            // Add daily to total
            totalCount += dailyCount;

            // Reset daily
            dailyCount = 0;

            // Restart 24-hour timer
            SaveResetTime();
            SaveData();
        }
    }

    void SaveResetTime()
    {
        PlayerPrefs.SetString(
            RESET_TIME_KEY,
            DateTime.Now.ToBinary().ToString()
        );
    }

    void LoadData()
    {
        totalCount = PlayerPrefs.GetInt(TOTAL_KEY, 0);
        dailyCount = PlayerPrefs.GetInt(DAILY_KEY, 0);
    }

    void SaveData()
    {
        PlayerPrefs.SetInt(TOTAL_KEY, totalCount);
        PlayerPrefs.SetInt(DAILY_KEY, dailyCount);
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        totalCountText.text = " " + totalCount;
        dailyCountText.text = " " + dailyCount;
    }
}

