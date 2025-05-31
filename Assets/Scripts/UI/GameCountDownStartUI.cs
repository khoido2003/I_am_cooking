using System;
using TMPro;
using UnityEngine;

public class GameCountDownStartUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countDownText;

    private void Start()
    {
        KichenGameManager.Instance.OnStateChanged += KitchenManager_OnStateChanged;

        Hide();
    }

    private void KitchenManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KichenGameManager.Instance.IsCountDownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        countDownText.text = Mathf
            .Ceil(KichenGameManager.Instance.GetCountDownToStartTime())
            .ToString();
    }
}
