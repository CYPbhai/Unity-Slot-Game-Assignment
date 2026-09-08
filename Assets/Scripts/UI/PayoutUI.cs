using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayoutUI : PopupUI
{
    [SerializeField] private TextMeshProUGUI payoutText;

    public void UpdatePayoutText(int money)
    {
        payoutText.text = "Payout: $" + money;
    }
}
