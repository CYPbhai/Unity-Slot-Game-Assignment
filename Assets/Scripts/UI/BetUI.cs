using UnityEngine;
using UnityEngine.UI;

public class BetUI : MonoBehaviour
{
    [SerializeField] private Button betButton100;
    [SerializeField] private Button betButton500;
    [Header("Raise Events")]
    [SerializeField] private IntChannelEventSO OnBetEvent;
    [Header("Subscribe Events")]
    [SerializeField] private VoidChannelEventSO OnPayoutEvent;
    private void Awake()
    {
        betButton100.onClick.AddListener(() =>
        {
            OnBetEvent?.Raise(100);
            DisableInteractivity();
        });
        betButton500.onClick.AddListener(() =>
        {
            OnBetEvent?.Raise(500);
            DisableInteractivity();
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
    private void OnPayoutEvent_OnRaised()
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
