using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class FLASH : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    private Coroutine animationCoroutine;
    void OnEnable()
    {
        animationCoroutine = StartCoroutine(AnimateLoadingText());
    }

    void OnDisable()
    {
        StopCoroutine(animationCoroutine);
    }
    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            buttonText.alpha = 1f;
            yield return new WaitForSeconds(0.5f);

            buttonText.alpha = 0.2f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
