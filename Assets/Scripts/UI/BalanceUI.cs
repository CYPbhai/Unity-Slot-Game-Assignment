using TMPro;
using UnityEngine;

public class BalanceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;

    [Header("Subscribe Events")]
    [SerializeField] private IntChannelEventSO OnBetEvent;
    [SerializeField] private IntChannelEventSO OnPayoutEvent;

    private void OnEnable()
    {
        UpdateBalanceUI();
        OnBetEvent.OnRaised += OnBetEvent_OnRaised;
        OnPayoutEvent.OnRaised += OnPayoutEvent_OnRaised;
    }
    private void OnDisable()
    {
        OnBetEvent.OnRaised -= OnBetEvent_OnRaised;
        OnPayoutEvent.OnRaised -= OnPayoutEvent_OnRaised;
    }
    private void OnPayoutEvent_OnRaised(int num)
    {
        UpdateBalanceUI();
    }

    private void OnBetEvent_OnRaised(int num)
    {
        UpdateBalanceUI();
    }

    private void UpdateBalanceUI()
    {
        balanceText.text = "$" + Balance.currentBalance;
    }
}
