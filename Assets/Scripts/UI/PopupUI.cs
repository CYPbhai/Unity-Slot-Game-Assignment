using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            StartCoroutine(ScaleAnimation(false)); // scale down
            closeButton.interactable = false;
        });
    }

    protected void Start()
    {
        // hide this UI in the start
        gameObject.SetActive(false);
    }


    protected void OnEnable()
    {
        closeButton.interactable = true;
        transform.localScale = Vector3.zero;
        StartCoroutine(ScaleAnimation(true)); // scale up
    }
    protected IEnumerator ScaleAnimation(bool isScaleUp)
    {
        Vector3 startScale = transform.localScale;
        float timer = 0f;
        float totalTime = 0.2f;
        while (timer / totalTime < 1)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, isScaleUp ? Vector3.one : Vector3.zero, timer / totalTime);
            yield return null;
        }
        transform.localScale = isScaleUp ? Vector3.one : Vector3.zero;
        if (!isScaleUp)
        {
            gameObject.SetActive(false);
        }
    }
}
