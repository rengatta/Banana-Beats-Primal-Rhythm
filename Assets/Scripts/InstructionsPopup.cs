using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//controls the instructions ui that briefly pops up during the start of each level
public class InstructionsPopup : MonoBehaviour
{

    public GameObject root;

    public Transform keyA;
    public Transform keyL;

     public Image keyAImage;
    public Image keyLImage;

    public Image handRightImage;
    public Image handLeftImage;

    public Image leftArrow;
    public Image rightArrow;

    public Image smartphoneLeft;
    public Image smartphoneRight;

    public Image screenSideLeft;
    public Image screenSideRight;


    float pulsateSpeed = 0.5f;
    float pulsateMin = 0.9f;
    float pulsateMax = 1.1f;
    float pulsateValue = 1.0f;

    float pulsateModifier = 1.0f;

    IEnumerator PulsateKeys() {
        while(true) {
            pulsateValue += pulsateSpeed * Time.deltaTime * pulsateModifier ;
            keyA.transform.localScale = new Vector3(1.0f * pulsateValue, 1.0f * pulsateValue, 1.0f);
            keyL.transform.localScale = new Vector3(1.0f * pulsateValue, 1.0f * pulsateValue, 1.0f);
            smartphoneLeft.transform.localScale = new Vector3(1.0f * pulsateValue, 1.0f * pulsateValue, 1.0f);
            smartphoneRight.transform.localScale = new Vector3(1.0f * pulsateValue, 1.0f * pulsateValue, 1.0f);

            if ((pulsateValue > pulsateMax && pulsateModifier == 1.0f) || (pulsateValue < pulsateMin && pulsateModifier == -1.0f)) {
                pulsateModifier = -pulsateModifier;
            }

            yield return new WaitForEndOfFrame();
        }

    }


    IEnumerator BlinkArrows() {
        while(true) {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            leftArrow.gameObject.SetActive(true);
            rightArrow.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

        }

    }


    IEnumerator DisableInstructions() {
        yield return new WaitForSeconds(8f);

        StartCoroutine(FadeAway());

    }

    float fadeSpeed = 0.7f;
    float alphaValue = 1.0f;


    IEnumerator FadeAway() {
        screenSideRight.color = new Color(screenSideRight.color.r, screenSideRight.color.g, screenSideRight.color.b, 0);
        screenSideLeft.color = new Color(screenSideLeft.color.r, screenSideLeft.color.g, screenSideLeft.color.b, 0);
        while (alphaValue > 0f) {
            alphaValue -= fadeSpeed * Time.deltaTime;

            keyAImage.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
            keyLImage.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
            handRightImage.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
            handLeftImage.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
            rightArrow.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
            leftArrow.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
            smartphoneLeft.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
            smartphoneRight.color = new Color(1.0f, 1.0f, 1.0f, alphaValue);

       

            yield return null;
        }
        StopAllCoroutines();
        root.SetActive(false);
    }


    void Start()
    {
        if (Application.platform == RuntimePlatform.Android || GameState.androidMode == true)
        {
            keyAImage.gameObject.SetActive(false);
            keyLImage.gameObject.SetActive(false);
            smartphoneLeft.gameObject.SetActive(true);
            smartphoneRight.gameObject.SetActive(true);
        } else {
            keyAImage.gameObject.SetActive(true);
            keyLImage.gameObject.SetActive(true);
            smartphoneLeft.gameObject.SetActive(false);
            smartphoneRight.gameObject.SetActive(false);

        }

        StartCoroutine(BlinkArrows());
        StartCoroutine(PulsateKeys());
        StartCoroutine(DisableInstructions());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
