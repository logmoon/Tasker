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
    [SerializeField] private Slider fireVolumeSlider;
    [SerializeField] private Slider rainVolumeSlider;
    [SerializeField] private GameObject fullscreenButton;
    [Header("Pause/Resume")]
    [SerializeField] private Image pauseResumeImage;
    [SerializeField] private GameObject pausedPanel;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite resumeSprite;
    [Header("Comfirmation Menu")]
    [SerializeField] private AlertMenuResetController comfirmationMenu;

    private bool timerStarted = false;
    private float currentTimer;

    private SessionData currentSession;

    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            fullscreenButton.SetActive(false);
        }

        Debug.Log(AudioManager.Instance.GetAudioVolume(AudioType.FIRE));
        fireVolumeSlider.value = AudioManager.Instance.GetAudioVolume(AudioType.FIRE);
        rainVolumeSlider.value = AudioManager.Instance.GetAudioVolume(AudioType.RAIN);
    }

    public void StartTimer (float _time, string _taskName)
    {
        // Data
        currentSession = new SessionData();
        currentSession.Name = _taskName;
        currentSession.SetDate(DateTime.Now);
        currentSession.Duration = _time;

        // Logic and UI
        timerStarted = true;
        taskText.text = _taskName;
        currentTimer = _time * 60.0f;
        pauseResumeImage.sprite = pauseSprite;
        pausedPanel.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Screen.fullScreen)
            {
                FullscreenButton();
            }
            else
            {
                QuitButton();
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PauseResumeButton();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            FullscreenButton();
        }

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

            GameManager.Instance.SaveData.Sessions.Add(currentSession);
            GameManager.Instance.SaveData.Save();
        }
    }


    public void FullscreenButton()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void PauseResumeButton()
    {
        timerStarted = !timerStarted;
        if (timerStarted)
        {
            pausedPanel.SetActive(false);
            pauseResumeImage.sprite = pauseSprite;
        }
        else
        {
            pausedPanel.SetActive(true);
            pauseResumeImage.sprite = resumeSprite;
        }
    }

    public void QuitButton()
    {
        // Setup the comfirmation menu first
        comfirmationMenu.SetupDialog("Are you sure you want to stop the session?",
        () =>
        {
            MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertReset, MenuIndexes.Setup, true);
            Time.timeScale = 1.0f;
        },
        () =>
        {
            MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertReset, MenuIndexes.Main, true);
            Time.timeScale = 1.0f;
        });

        // Then show it and pause
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Main, MenuIndexes.AlertReset, true);
        Time.timeScale = 0.0f;
    }
    public void FireButton()
    {
        if (GameManager.Instance.FirePlaying)
        {
            AudioManager.Instance.StopAudio(AudioType.FIRE, true);
        }
        else
        {
            AudioManager.Instance.PlayAudio(AudioType.FIRE, true);
            SetFireVolume();
        }

        GameManager.Instance.FirePlaying = !GameManager.Instance.FirePlaying;
    }
    public void SetFireVolume()
    {
        AudioManager.Instance.SetAudioVolume(AudioType.FIRE, fireVolumeSlider.value);
    }

    public void RainButton()
    {
        if (GameManager.Instance.RainPlaying)
        {
            AudioManager.Instance.StopAudio(AudioType.RAIN, true);
        }
        else
        {
            AudioManager.Instance.PlayAudio(AudioType.RAIN, true);
            SetRainVolume();
        }

        GameManager.Instance.RainPlaying = !GameManager.Instance.RainPlaying;
    }
    public void SetRainVolume()
    {
        AudioManager.Instance.SetAudioVolume(AudioType.RAIN, rainVolumeSlider.value);
    }

}
