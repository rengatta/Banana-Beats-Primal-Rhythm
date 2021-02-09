using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSliderFailBox : MonoBehaviour
{
    public GameObject endSlider;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == GlobalHelper.sliderLayer && collision.gameObject == endSlider)
        {

            HoldSlider parentScript = collision.transform.parent.GetComponent<HoldSlider>();
            parentScript.TriggerFail();
        }
    }
}
