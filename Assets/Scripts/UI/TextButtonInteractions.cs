using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class TextButtonInteractions : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI buttonText;
    public string enteredText;

    private Coroutine animationCoroutine;


    void Awake()
    {
        enteredText = buttonText.text;
    }

    void OnEnable()
    {
        buttonText.text = enteredText;
        buttonText.alpha = 1f;
    }


    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonSelectSfx);
        Debug.Log("selected");
        buttonText.text = ">" + enteredText + "<";
        animationCoroutine = StartCoroutine(AnimateLoadingText());
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("de-selected");
        buttonText.text = enteredText;
        StopCoroutine(animationCoroutine);
        buttonText.alpha = 1;
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
