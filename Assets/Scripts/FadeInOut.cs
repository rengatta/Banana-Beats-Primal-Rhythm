using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//controls a black image that covers the entire scene screen
//the image will start black and slowly increase in transparency
//a unique coroutine may be passed in to trigger once the image is fully transparent
public class FadeInOut : MonoBehaviour
{
    public Image fadeoutImage;
    public float fadeOutSpeed = 5f;
    public float fadeInSpeed = 5f;
    public float fadeInPause = 1f;

    IEnumerator SceneFadeOut(IEnumerator otherCoroutine)
    {
        fadeoutImage.raycastTarget = true;
        while (fadeoutImage.color.a < 1.0f)
        {
            fadeoutImage.color += new Color(0f, 0f, 0f, fadeOutSpeed * Time.unscaledDeltaTime);

            yield return null;
        }
        StartCoroutine(otherCoroutine);
    }

    public void FadeOut(IEnumerator otherCoroutine) {
        StartCoroutine(SceneFadeOut(otherCoroutine));
    }


    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(SceneFadeIn());
    }

    IEnumerator SceneFadeIn()
    {
        yield return new WaitForSeconds(fadeInPause);
        fadeoutImage.color = new Color(fadeoutImage.color.r, fadeoutImage.color.g, fadeoutImage.color.b, 1.0f);
        fadeoutImage.raycastTarget = true;
        while (fadeoutImage.color.a > 0f)
        {
            fadeoutImage.color -= new Color(0f, 0f, 0f, fadeInSpeed * Time.unscaledDeltaTime);

            yield return null;
        }
        fadeoutImage.raycastTarget = false;

    }
}
