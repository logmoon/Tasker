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
    private bool gamePaused = false;
    private bool firePlaying = false;
    private bool rainPlaying = false;
    private float currentTimer;
    private float timerInMinutes;

    public void StartTimer (float _time, string _taskName)
    {
        timerStarted = true;
        taskText.text = "Task: " + _taskName;
        finishedTasksText.text = completedSessionsText + PlayerPrefs.GetInt("completed_tasks");
        timerInMinutes = _time;
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
        timerText.text = (Mathf.FloorToInt(currentTimer / 60) + 1).ToString();

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
        if (gamePaused)
        {
            MenuManager.Instance.TurnMenuOff(MenuIndexes.Pause, MenuIndexes.None, true);
        }
        else
        {
            MenuManager.Instance.TurnMenuOn(MenuIndexes.Pause);
        }

        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0.0f : 1.0f;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ResetButton()
    {
        timerStarted = false;
        timerText.text = "0";
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Main, MenuIndexes.Setup, true);
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
