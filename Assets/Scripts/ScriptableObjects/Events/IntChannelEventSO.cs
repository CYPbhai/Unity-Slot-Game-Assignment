using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Int Channel Event", fileName = "Int Event")]
public class IntChannelEventSO : ScriptableObject
{
    public event Action<int> OnRaised;

    public void Raise(int number)
    {
        OnRaised?.Invoke(number);
    }
}
