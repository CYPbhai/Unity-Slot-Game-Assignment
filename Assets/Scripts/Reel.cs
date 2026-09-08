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

    [Header("Symbols (assign in enum order: Seven, Cherry, Bell, Bar)")]
    [SerializeField] private Transform[] symbolSprites;

    [Header("Win Animation")]
    [SerializeField] private float punchScale = 1.25f;
    [SerializeField] private float punchDuration = 0.35f;
    [SerializeField] private int punchCount = 2;

    [Header("Raise Events")]
    [SerializeField] private SlotChannelEventSO OnReelStopped;

    [Header("Subscribe Events")]
    [SerializeField] private IntChannelEventSO OnBet;
    [SerializeField] private SlotChannelEventSO OnWinningSlot;

    private Slot stoppedSlot = Slot.None;
    private Coroutine winAnimRoutine;

    private void OnEnable()
    {
        OnBet.OnRaised += OnBet_OnRaised;
        OnWinningSlot.OnRaised += OnWinningSlot_OnRaised;

        int rnd = Random.Range(0, 8);
        if (rnd % 2 != 0)
        {
            rnd += 1;
        }
        transform.position = new Vector2(transform.position.x, rnd);
    }

    private void OnDisable()
    {
        OnBet.OnRaised -= OnBet_OnRaised;
        OnWinningSlot.OnRaised -= OnWinningSlot_OnRaised;
    }

    private void OnBet_OnRaised(int number)
    {
        StartSpin();
    }

    private void OnWinningSlot_OnRaised(Slot winningSlot)
    {
        // Only play the animation if THIS reel actually landed on the winning symbol.
        if (winningSlot == stoppedSlot)
            PlayWinAnimation();
    }

    private void StartSpin()
    {
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        ResetWinAnimation(); // clear any leftover pulse from the previous round
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

    // Converts a Slot enum value back into an array index
    private int SlotToIndex(Slot slot)
    {
        return Mathf.RoundToInt((int)slot / slotSpacing);
    }

    private void PlayWinAnimation()
    {
        int index = SlotToIndex(stoppedSlot);
        if (symbolSprites == null || index < 0 || index >= symbolSprites.Length)
            return;

        if (winAnimRoutine != null)
            StopCoroutine(winAnimRoutine);

        winAnimRoutine = StartCoroutine(PunchScale(symbolSprites[index]));
    }

    private void ResetWinAnimation()
    {
        if (winAnimRoutine != null)
        {
            StopCoroutine(winAnimRoutine);
            winAnimRoutine = null;
        }

        if (symbolSprites == null) return;

        foreach (var symbol in symbolSprites)
        {
            if (symbol != null)
                symbol.localScale = Vector3.one;
        }
    }

    private IEnumerator PunchScale(Transform target)
    {
        Vector3 originalScale = target.localScale;

        for (int i = 0; i < punchCount; i++)
        {
            float t = 0f;
            while (t < punchDuration)
            {
                t += Time.deltaTime;
                float sine = Mathf.Sin((t / punchDuration) * Mathf.PI); // 0 -> 1 -> 0
                // originalScale -> originalScale * punchScale -> originalScale
                target.localScale = originalScale * (1f + sine * (punchScale - 1f));
                yield return null;
            }
        }

        target.localScale = originalScale;
    }
}