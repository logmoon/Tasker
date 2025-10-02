using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetupMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuController mainMenu;
    [SerializeField] private AlertMenuResetController comfirmationMenu;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI taskNameText;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private CustomButton startSessionButton;

    [SerializeField] private Slider fireVolumeSlider;
    [SerializeField] private Slider rainVolumeSlider;

    private float timer;

    private void OnEnable()
    {
        titleText.text = WelcomeMessageGenerator.GetMessage();
        inputText.text = "";
        taskNameText.text = "";

        timer = 0.0f;

        fireVolumeSlider.value = AudioManager.Instance.GetAudioVolume(AudioType.FIRE);
        rainVolumeSlider.value = AudioManager.Instance.GetAudioVolume(AudioType.RAIN);
    }

    private bool IsValidInput(out float time)
    {
        time = 0.0f;
        if (taskNameText.text.Length <= 1) return false;
        string s = inputText.text.Substring(0, inputText.text.Length - 1); 
        return float.TryParse(s, out time);
    }

    private void Update()
    {
        startSessionButton.CanPress = IsValidInput(out float time);

        timer += Time.deltaTime;

        // Convert the float timer value to a TimeSpan
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
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
    }

    public void StartButton()
    {
        float time = 0.0f;
        if (IsValidInput(out time))
        {
            mainMenu.StartTimer(time, taskNameText.text);
            MenuManager.Instance.TurnMenuOff(MenuIndexes.Setup, MenuIndexes.Main, true);
        }
    }
    public void ProgressButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Setup, MenuIndexes.Progress, true);
    }

    public void FullscreenButton()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SettingsButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Setup, MenuIndexes.Settings, true);
    }

    public void QuitButton()
    {
        // Setup the comfirmation menu first
        comfirmationMenu.SetupDialog("Are you sure you want to quit?",
        () =>
        {
            Application.Quit();
        },
        () =>
        {
            MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertReset, MenuIndexes.Setup, true);
        });

        // Then show it
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Setup, MenuIndexes.AlertReset, true);
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
