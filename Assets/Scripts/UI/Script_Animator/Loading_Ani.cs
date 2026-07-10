using UnityEngine;
using TMPro;
using System.Collections;

public class Loading_Ani : MonoBehaviour
{
    [Header("UI Component")]
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Animation Settings")]
    [SerializeField] private string baseText = "Loading Game";
    [SerializeField] private float dotSpeed = 0.5f;

    private Coroutine animationCoroutine;

    private void OnEnable()
    {
        // Start the loop when the object is active
        animationCoroutine = StartCoroutine(AnimateLoadingText());
    }

    private void OnDisable()
    {
        // Stop the loop if the screen is hidden
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
    }

    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            loadingText.text = baseText;
            yield return new WaitForSeconds(dotSpeed);

            loadingText.text = baseText + ".";
            yield return new WaitForSeconds(dotSpeed);

            loadingText.text = baseText + "..";
            yield return new WaitForSeconds(dotSpeed);

            loadingText.text = baseText + "...";
            yield return new WaitForSeconds(dotSpeed);
        }
    }
}
