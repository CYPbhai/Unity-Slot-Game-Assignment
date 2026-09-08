using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Subscribe Events")]
    [SerializeField] private IntChannelEventSO OnPayout;
    [SerializeField] private PayoutUI payOutUI;

    private void OnEnable()
    {
        OnPayout.OnRaised += OnPayout_OnRaised;
    }

    // NOTE(CYPbhai): it's not a good practice to update global data in UI scripts
    // But we used it because we don't want to create new script for only handling Balance when paying out.
    // We should create separate script if we want to scale the code
    private void OnPayout_OnRaised(int money)
    {
        Balance.currentBalance += money;
        payOutUI.gameObject.SetActive(true);
        payOutUI.UpdatePayoutText(money);
    }
}
