using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class TextButtonInteractions : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI buttonText;
    public string enteredText;

    private Coroutine animationCoroutine;


    void Start()
    {
        enteredText = buttonText.text;
    }


    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selected");
        buttonText.text = ">" + enteredText + "<";
        animationCoroutine = StartCoroutine(AnimateLoadingText());
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("de-selected");
        buttonText.text = enteredText;
        StopCoroutine(animationCoroutine);
        buttonText.alpha = 255;
    }

    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            buttonText.alpha = 0.3f;
            yield return new WaitForSeconds(0.5f);

            buttonText.alpha = 1f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
