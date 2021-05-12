using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//part of the failbox prefab that gets instantiated whenever a hold note is activated
//when the marker enters this failbox, the note will be failed and a "Miss" will occur
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
