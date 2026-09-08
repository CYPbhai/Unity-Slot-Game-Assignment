using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Slot Channel Event", fileName = "Slot Event")]
public class SlotChannelEventSO : ScriptableObject
{
    public event Action<Slot> OnRaised;

    public void Raise(Slot slot)
    {
        OnRaised?.Invoke(slot);
    }
}
