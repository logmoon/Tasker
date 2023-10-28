using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI taskText;
    [SerializeField] private TextMeshProUGUI finishedTasksText;
    [SerializeField] private string completedSessionsText;
    [SerializeField] private Slider fireVolumeSlider;
    [SerializeField] private Slider rainVolumeSlider;

    private bool timerStarted = false;
    private bool firePlaying = false;
    private bool rainPlaying = false;
    private float currentTimer;

    private void OnEnable()
    {
        finishedTasksText.text = completedSessionsText + PlayerPrefs.GetInt("completed_tasks");
    }

    public void StartTimer (float _time, string _taskName)
    {
        timerStarted = true;
        taskText.text = "Task: " + _taskName;
        currentTimer = _time * 60.0f;
    }

    private void Awake()
    {
        firePlaying = false;
        rainPlaying = false;
    }


    private void Update()
    {
        timeText.text = DateTime.Now.ToString("hh:mm tt");

        if (!timerStarted) return;
        currentTimer -= Time.deltaTime;

        // Convert the float timer value to a TimeSpan
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTimer);
        // Format the TimeSpan
        string formattedTimer;
        if (timeSpan.Hours >= 1)
        {
            // Format the TimeSpan as "hh:mm:ss"
            formattedTimer = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else
        {
            // Format the TimeSpan as "mm:ss" when it's less than an hour
            formattedTimer = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }
        timerText.text = formattedTimer;

        if (currentTimer <= 0 )
        {
            // Timer finished
            timerStarted = false;
            timerText.text = "0";
            AudioManager.Instance.PlayAudio(AudioType.TIMER_OUT);
            MenuManager.Instance.TurnMenuOff(MenuIndexes.Main, MenuIndexes.Setup, true);
            int tasks = PlayerPrefs.GetInt("completed_tasks") + 1;
            PlayerPrefs.SetInt("completed_tasks", tasks);
            finishedTasksText.text = completedSessionsText + tasks;
        }
    }

    public void FullscreenButton()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void PauseButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Main, MenuIndexes.Pause, true);
        Time.timeScale = 0.0f;
    }

    public void QuitButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Main, MenuIndexes.AlertQuit, true);
        Time.timeScale = 0.0f;
    }

    public void ResetButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Main, MenuIndexes.AlertReset, true);
        Time.timeScale = 0.0f;
    }

    public void FireButton()
    {
        if (firePlaying)
        {
            AudioManager.Instance.StopAudio(AudioType.FIRE, true);
        }
        else
        {
            AudioManager.Instance.PlayAudio(AudioType.FIRE, true);
            SetFireVolume();
        }

        firePlaying = !firePlaying;
    }
    public void SetFireVolume()
    {
        AudioManager.Instance.SetAudioVolume(AudioType.FIRE, fireVolumeSlider.value);
    }

    public void RainButton()
    {
        if (rainPlaying)
        {
            AudioManager.Instance.StopAudio(AudioType.RAIN, true);
        }
        else
        {
            AudioManager.Instance.PlayAudio(AudioType.RAIN, true);
            SetRainVolume();
        }

        rainPlaying = !rainPlaying;
    }
    public void SetRainVolume()
    {
        AudioManager.Instance.SetAudioVolume(AudioType.RAIN, rainVolumeSlider.value);
    }
}
