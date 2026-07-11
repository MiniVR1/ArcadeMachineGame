using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SliderInteraction : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image handle;
    private Color colorFade = Color.white;
    private Color colorNonFade = Color.white;

    private Coroutine animationCoroutine;

    private void Start()
    {
        colorFade.a = 0.25f;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonSelectSfx);
        Debug.Log("selected Slider");
        animationCoroutine = StartCoroutine(AnimateLoadingText());
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("de-selected Slider");
        StopCoroutine(animationCoroutine);
        handle.color = colorNonFade;
    }

    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            handle.color = colorNonFade;
            yield return new WaitForSeconds(0.5f);

            handle.color = colorFade;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
