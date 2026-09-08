using UnityEngine;

public class SlotMachineVisual : MonoBehaviour
{
    [Header("Subscribe Events")]
    [SerializeField] private IntChannelEventSO OnBetEvent;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        OnBetEvent.OnRaised += OnBetEvent_OnRaised;
    }
    private void OnDisable()
    {
        OnBetEvent.OnRaised -= OnBetEvent_OnRaised;
    }

    private void OnBetEvent_OnRaised(int number)
    {
        animator?.SetTrigger("Bet");
    }
}
