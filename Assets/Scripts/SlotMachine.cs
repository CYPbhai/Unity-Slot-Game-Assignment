using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [Header("Raise Events")]
    [SerializeField] private IntChannelEventSO OnPayout;

    [Header("Subscribe Events")]
    [SerializeField] private SlotChannelEventSO OnReel1Stopped;
    [SerializeField] private SlotChannelEventSO OnReel2Stopped;
    [SerializeField] private SlotChannelEventSO OnReel3Stopped;
    [SerializeField] private IntChannelEventSO OnBet;

    private int betAmount;
    private Slot[] finalSlots = {Slot.None, Slot.None, Slot.None };
    private int count = 0;
    private int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
            if(count == 3)
            {
                count = 0;
                Payout();
            }
        }
    }
    private void OnEnable()
    {
        OnReel1Stopped.OnRaised += OnReel1Stopped_OnRaised;
        OnReel2Stopped.OnRaised += OnReel2Stopped_OnRaised;
        OnReel3Stopped.OnRaised += OnReel3Stopped_OnRaised;
        OnBet.OnRaised += OnBet_OnRaised;
    }

    private void OnBet_OnRaised(int bet)
    {
        betAmount = bet;
    }

    private void OnReel3Stopped_OnRaised(Slot slot)
    {
        finalSlots[2] = slot;
        Count++;
    }

    private void OnReel2Stopped_OnRaised(Slot slot)
    {
        finalSlots[1] = slot;
        Count++;
    }

    private void OnReel1Stopped_OnRaised(Slot slot)
    {
        finalSlots[0] = slot;
        Count++;
    }

    private void Payout()
    {
        int cherryMultiplier = 2;
        int barMultiplier = 3;
        int bellMultiplier = 5;
        int sevenMultiplier = 10;

        int cherryDivisor = 5;
        int barDivisor = 3;
        int bellDivisor = 2;
        int sevenDivisor = 1;

        Slot slot1 = finalSlots[0];
        Slot slot2 = finalSlots[1];
        Slot slot3 = finalSlots[2];

        if ((slot1 == slot2) && (slot2 == slot3))
        {
            switch(slot1)
            {
                case Slot.Cherry:
                    {
                        OnPayout?.Raise(betAmount * cherryMultiplier);
                    }
                    break;
                
                case Slot.Bar:
                    {
                        OnPayout?.Raise(betAmount * barMultiplier);
                    }
                    break;
                
                case Slot.Bell:
                    {
                        OnPayout?.Raise(betAmount * bellMultiplier);
                    }
                    break;
                
                case Slot.Seven:
                    {
                        OnPayout?.Raise(betAmount * sevenMultiplier);
                    }
                    break;
                
            }
        }
        else if((slot1 == slot2) || slot3 == slot1)
        {
            switch (slot1)
            {
                case Slot.Cherry:
                    {
                        OnPayout?.Raise(betAmount/ cherryDivisor);
                    }
                    break;

                case Slot.Bar:
                    {
                        OnPayout?.Raise(betAmount/barDivisor);
                    }
                    break;

                case Slot.Bell:
                    {
                        OnPayout?.Raise(betAmount/bellDivisor);
                    }
                    break;

                case Slot.Seven:
                    {
                        OnPayout?.Raise(betAmount/sevenDivisor);
                    }
                    break;

            }
        }
        else if((slot2 == slot3))
        {
            switch (slot2)
            {
                case Slot.Cherry:
                    {
                        OnPayout?.Raise(betAmount / cherryDivisor);
                    }
                    break;

                case Slot.Bar:
                    {
                        OnPayout?.Raise(betAmount / barDivisor);
                    }
                    break;

                case Slot.Bell:
                    {
                        OnPayout?.Raise(betAmount / bellDivisor);
                    }
                    break;

                case Slot.Seven:
                    {
                        OnPayout?.Raise(betAmount / sevenDivisor);
                    }
                    break;
            }
        }
        else
        {
            OnPayout?.Raise(0);
        }
    }
}
