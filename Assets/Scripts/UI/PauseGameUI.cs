using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameUI : MonoBehaviour
{
    [SerializeField]
    private Button resumeBtn;

    [SerializeField]
    private Button mainMenuBtn;

    [SerializeField]
    private Button optionBtn;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            KichenGameManager.Instance.TogglePauseGame();
        });

        mainMenuBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MenuScene);
        });

        optionBtn.onClick.AddListener(() => {
OptionUI.Instance.Show();
                });
    }

    private void Start()
    {
        KichenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;

        KichenGameManager.Instance.OnGameUnPaused += KitchenGameManager_OnGameUnPaused;

        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void KitchenGameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
