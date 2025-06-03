using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI playerTimerText;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
        playerTimerText.gameObject.SetActive(true);
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {        
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            Show();
           
        }
        else
        {
            Hide();
        }        

    }

    private void Update()
    {
        // String format zero decimal places
        countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();

        
        playerTimerText.text = KitchenGameManager.Instance.GetGamePlayingTimer().ToString("F0");
    }

    private void Show()
    {
        countdownText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }
  
        
}
