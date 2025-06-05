using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }

    [SerializeField]
    private Button soundEffectBtn;

    [SerializeField]
    private Button closeBtn;

    [SerializeField]
    private Button musicBtn;

    [SerializeField]
    private TextMeshProUGUI soundEffectText;

    [SerializeField]
    private TextMeshProUGUI musicText;

    private void Awake()
    {
        Instance = this;

        soundEffectBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeBtn.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        KichenGameManager.Instance.OnGamePaused += KitchenGameManger_OnGamePaused;
        UpdateVisual();

        Hide();
    }

    private void KitchenGameManger_OnGamePaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text =
            "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);

        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
