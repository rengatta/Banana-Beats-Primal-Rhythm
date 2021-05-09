using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGlowHolder : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    float intensity = 4.5f;
    public void Init(Vector3 position, Color color) {
        spriteRenderer.color = color;

        this.transform.position = position;
        spriteRenderer.material.SetFloat("_Intensity", intensity);


    }

    public void FadeOut() {
        StartCoroutine(GlowFadeOut());

    }


    public void FadeOutBlack() {
        StartCoroutine(GlowFadeOutBlack());

    }

    float blackoutSpeed = 6.0f;
    IEnumerator GlowFadeOutBlack()
    {

        while (spriteRenderer.color.a > 0f) {
            spriteRenderer.color -= new Color(blackoutSpeed * Time.deltaTime, blackoutSpeed * Time.deltaTime, blackoutSpeed * Time.deltaTime, fadeSpeed * Time.deltaTime);

            yield return null;
        }
        Destroy(this.gameObject);
    }

    float fadeSpeed = 10f;
    IEnumerator GlowFadeOut()
    {
        intensity += 1.3f;
        spriteRenderer.material.SetFloat("_Intensity", intensity);
        spriteRenderer.color += new Color(0.05f, 0.05f, 0.05f, 0.0f);
        while (spriteRenderer.color.a > 0f)
        {
            spriteRenderer.color -= new Color(0f, 0f, 0f, fadeSpeed * Time.deltaTime);

            yield return null;
        }
        Destroy(this.gameObject);
    }

}
