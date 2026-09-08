using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Channel Event", fileName = "Void Event")]
public class VoidChannelEventSO : ScriptableObject
{
    public event Action OnRaised;

    public void Raise()
    {
        OnRaised?.Invoke();
    }
}
