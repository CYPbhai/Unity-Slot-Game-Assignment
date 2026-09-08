using System.Collections;
using UnityEngine;

// Slot enum based on y position to make it clear
public enum Slot
{
    None = -1,
    Seven = 0,
    Cherry = 2,
    Bell = 4,
    Bar = 6
}

public class Reel : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private float slotSpacing = 2f;
    [SerializeField] private int slotCount = 4;
    private float WrapHeight => slotSpacing * slotCount; // 8

    [Header("Motion")]
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float accelTime = 1f;
    [SerializeField] private float minSpinTime = 2f;
    [SerializeField] private float decelTime = 1f;
    [SerializeField] private float snapSpeed = 2f;

    [Header("Raise Events")]
    [SerializeField] private SlotChannelEventSO OnReelStopped;

    [Header("Subscribe Events")]
    [SerializeField] private IntChannelEventSO OnBet;

    private Slot stoppedSlot = Slot.None;

    private void OnEnable()
    {
        OnBet.OnRaised += OnBet_OnRaised; ;
        int rnd = Random.Range(0, 8);
        if(rnd%2 !=0)
        {
            rnd += 1;
        }
        transform.position = new Vector2(transform.position.x, rnd);
    }

    private void OnDisable()
    {
        OnBet.OnRaised -= OnBet_OnRaised;
    }

    private void OnBet_OnRaised(int number)
    {
        StartSpin();
    }

    private void StartSpin()
    {
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        stoppedSlot = Slot.None;

        int landingIndex = Random.Range(0, slotCount);
        float targetY = landingIndex * slotSpacing;

        float elapsed = 0f;

        // Starting movement smoothly
        while (elapsed < accelTime)
        {
            elapsed += Time.deltaTime;
            float speed = Mathf.Lerp(0f, maxSpeed, elapsed / accelTime);
            Move(speed * Time.deltaTime);
            yield return null;
        }

        // Moving with constant speed
        elapsed = 0f;
        float spinTime = minSpinTime + Random.Range(0f, 0.6f);
        while (elapsed < spinTime)
        {
            elapsed += Time.deltaTime;
            Move(maxSpeed * Time.deltaTime);
            yield return null;
        }

        // Stopping movement smoothly
        elapsed = 0f;
        while (elapsed < decelTime)
        {
            elapsed += Time.deltaTime;
            float speed = Mathf.Lerp(maxSpeed, snapSpeed, elapsed / decelTime);
            Move(speed * Time.deltaTime);
            yield return null;
        }

        // it allows us to move only in positive direction
        float remaining = Mathf.Repeat(targetY - transform.position.y, WrapHeight);

        while (remaining > 0.01f)
        {
            float step = Mathf.Min(remaining, remaining * snapSpeed * Time.deltaTime);
            remaining -= step;
            Move(step);
            yield return null;
        }

        // ensuring that final position is set correctly
        transform.position = new Vector2(transform.position.x, targetY);
        stoppedSlot = (Slot)Mathf.RoundToInt(targetY); // Based on enum
        OnReelStopped?.Raise(stoppedSlot);
    }

    private void Move(float delta)
    {
        Vector2 pos = transform.position;
        pos.y += delta;

        // if it reaches bottum slot of the reel - it wraps it to starting position
        if (pos.y >= WrapHeight)
            pos.y -= WrapHeight;

        transform.position = pos;
    }
}