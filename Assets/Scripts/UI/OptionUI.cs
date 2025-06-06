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

    [SerializeField]
    private TextMeshProUGUI moveUpText;

    [SerializeField]
    private TextMeshProUGUI moveRightText;

    [SerializeField]
    private TextMeshProUGUI moveDownText;

    [SerializeField]
    private TextMeshProUGUI moveLeftText;

    [SerializeField]
    private TextMeshProUGUI interactText;

    [SerializeField]
    private TextMeshProUGUI interactAltText;

    [SerializeField]
    private TextMeshProUGUI pauseText;

    [SerializeField]
    private Button moveUpBtn;

    [SerializeField]
    private Button moveRightBtn;

    [SerializeField]
    private Button moveDownBtn;

    [SerializeField]
    private Button moveLeftBtn;

    [SerializeField]
    private Button interactBtn;

    [SerializeField]
    private Button interactAltBtn;

    [SerializeField]
    private Button pauseBtn;

    [SerializeField]
    private Transform pressToRebindKey;

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

        moveDownBtn.onClick.AddListener(() =>
        {
            Rebinding(GameInput.Binding.Move_Down);
        });
        moveUpBtn.onClick.AddListener(() =>
        {
            Rebinding(GameInput.Binding.Move_Up);
        });
        moveRightBtn.onClick.AddListener(() =>
        {
            Rebinding(GameInput.Binding.Move_Right);
        });
        moveLeftBtn.onClick.AddListener(() =>
        {
            Rebinding(GameInput.Binding.Move_Left);
        });
        interactBtn.onClick.AddListener(() =>
        {
            Rebinding(GameInput.Binding.Interact);
        });
        interactAltBtn.onClick.AddListener(() =>
        {
            Rebinding(GameInput.Binding.InteractAlt);
        });
        pauseBtn.onClick.AddListener(() =>
        {
            Rebinding(GameInput.Binding.Pause);
        });
    }

    private void Start()
    {
        KichenGameManager.Instance.OnGamePaused += KitchenGameManger_OnGamePaused;
        UpdateVisual();

        Hide();
        HidePressToRebindKey();
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

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKey.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKey.gameObject.SetActive(false);
    }

    private void Rebinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();

        GameInput.Instance.RebindingKeyMap(
            binding,
            () =>
            {
                HidePressToRebindKey();
                UpdateVisual();
            }
        );
    }
}
