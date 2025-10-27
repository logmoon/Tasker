using Keiwando.NFSO;
using TMPro;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Application")]
    [SerializeField] private TextMeshProUGUI currentFpsCapText;
    [Header("Data")]
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private AlertMenuResetController comfirmationMenu;

    private float messageTimer;
    private float messageDuration;

    SupportedFileType saveFileType = new SupportedFileType
    {
        Name = "SaveFile",
        Extension = "save",
        Owner = false,
        AppleUTI = "public.plain-text",
        MimeType = "text/plain"
    };

    private void OnEnable()
    {
        message.gameObject.SetActive(false);

        currentFpsCapText.text = GameManager.Instance.SaveData.FPSCapSetting switch
        {
            FPSCapSetting.THIRTY => "Currently: {30}",
            FPSCapSetting.SIXTY => "Currently: {60}",
            FPSCapSetting.MONITOR => $"Currently: {{{GameManager.Instance.GetFpsCapForMonitor()}}}",
            _ => currentFpsCapText.text
        };
    }

    private void Update()
    {
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;

            // Scale up slightly during display (1.05x)
            float progress = 1f - (messageTimer / messageDuration);
            float scale = Mathf.Lerp(1f, 1.05f, progress);

            message.transform.localScale = Vector3.one * scale;

            // Fade out in the last 0.3 seconds
            if (messageTimer < 0.3f)
            {
                Color col = message.color;
                col.a = messageTimer / 0.3f;
                message.color = col;
            }

            if (messageTimer <= 0)
            {
                message.gameObject.SetActive(false);
                message.transform.localScale = Vector3.one;
            }
        }
    }

    private void ShowMessage(string text)
    {
        if (message == null) return;

        message.text = text;
        message.gameObject.SetActive(true);

        // Duration based on message length (0.5s per 10 characters, min 1.5s, max 3.5s)
        messageDuration = Mathf.Clamp(1.5f + (text.Length / 10f) * 0.5f, 1.5f, 3.5f);
        messageTimer = messageDuration;

        // Reset alpha
        Color col = message.color;
        col.a = 1f;
        message.color = col;
    }

    public void BackButton()
    {
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Settings, MenuIndexes.Setup, true);
    }

    public void ExportDataButton()
    {
        string path = GameManager.Instance.SaveData.GetDefaultPath();
        string newFilename = "ExportedTaskerSessions.save";

        FileToSave file = new FileToSave(path, newFilename, saveFileType);

        // Allows the user to choose a save location and saves the 
        // file to that location
        NativeFileSO.shared.SaveFile(file);
        ShowMessage("Data exported successfully!");
    }
    public void ImportDataButton()
    {
        // We want the user to select a plain text file.
        SupportedFileType[] supportedFileTypes = {
            saveFileType
        };

        NativeFileSO.shared.OpenFile(supportedFileTypes,
            delegate (bool fileWasOpened, OpenedFile file)
            {
                if (fileWasOpened)
                {
                    // Load the data
                    GameManager.Instance.SaveData.LoadFromData(file.ToUTF8String());
                    GameManager.Instance.SaveData.Save();
                    ShowMessage("Data imported successfully!");
                }
                else
                {
                    // The file selection was cancelled.
                }
            }
        );
    }
    public void DeleteDataButton()
    {
        // Setup the comfirmation menu first
        comfirmationMenu.SetupDialog("Are you sure you want to delete your data? All your saved sessions and progress will be lost.",
        () =>
        {
            GameManager.Instance.VoidSaveData();
            MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertReset, MenuIndexes.Settings, false);
            ShowMessage("All data deleted");
        },
        () =>
        {
            MenuManager.Instance.TurnMenuOff(MenuIndexes.AlertReset, MenuIndexes.Settings, true);
        });

        // Then show it
        MenuManager.Instance.TurnMenuOff(MenuIndexes.Settings, MenuIndexes.AlertReset, true);
    }

    public void ThirtyFpsButton()
    {
        GameManager.Instance.SetFpsCap(FPSCapSetting.THIRTY, true);
        currentFpsCapText.text = "Currently: {30}";
    }
    public void SixtyFpsButton()
    {
        GameManager.Instance.SetFpsCap(FPSCapSetting.SIXTY, true);
        currentFpsCapText.text = "Currently: {60}";
    }
    public void MonitorFpsButton()
    {
        GameManager.Instance.SetFpsCap(FPSCapSetting.MONITOR, true);
        currentFpsCapText.text = $"Currently: {{{GameManager.Instance.GetFpsCapForMonitor()}}}";
    }
}
