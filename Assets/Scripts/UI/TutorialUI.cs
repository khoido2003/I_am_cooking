using System;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI keyMoveUpText;

    [SerializeField]
    private TextMeshProUGUI keyMoveDownText;

    [SerializeField]
    private TextMeshProUGUI keyMoveLeftText;

    [SerializeField]
    private TextMeshProUGUI keyMoveRightText;

    [SerializeField]
    private TextMeshProUGUI keyInteractText;

    [SerializeField]
    private TextMeshProUGUI keyInteractAltText;

    [SerializeField]
    private TextMeshProUGUI keyPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;

        KichenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateView();

        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KichenGameManager.Instance.IsCountDownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateView();
    }

    private void UpdateView()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
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
