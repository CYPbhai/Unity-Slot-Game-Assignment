using UnityEngine;
using UnityEngine.UI;

public class BetUI : MonoBehaviour
{
    [SerializeField] private Button betButton100;
    [SerializeField] private Button betButton500;
    [Header("Raise Events")]
    [SerializeField] private IntChannelEventSO OnBetEvent;
    [Header("Subscribe Events")]
    [SerializeField] private IntChannelEventSO OnPayoutEvent;
    private void Awake()
    {
        betButton100.onClick.AddListener(() =>
        {
            if(Balance.currentBalance >= 100)
            {
                Balance.currentBalance -= 100;
                OnBetEvent?.Raise(100);
                DisableInteractivity();
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        });
        betButton500.onClick.AddListener(() =>
        {
            if (Balance.currentBalance >= 500)
            {
                Balance.currentBalance -= 500;
                OnBetEvent?.Raise(500);
                DisableInteractivity();
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        });
    }

    private void OnEnable()
    {
        OnPayoutEvent.OnRaised += OnPayoutEvent_OnRaised;
    }
    private void OnDisable()
    {
        OnPayoutEvent.OnRaised -= OnPayoutEvent_OnRaised;
    }
    private void OnPayoutEvent_OnRaised(int num)
    {
        EnableInteractivity();
    }

    private void DisableInteractivity()
    {
        betButton100.interactable = false;
        betButton500.interactable = false;
    }

    private void EnableInteractivity()
    {
        betButton100.interactable = true;
        betButton500.interactable = true;
    }
}
