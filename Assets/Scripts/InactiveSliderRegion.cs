using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InactiveSliderRegion : MonoBehaviour
{
    public TextMeshProUGUI hitScore;
    public Smileys smileys;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            HorizontalSlider sliderInstance = collision.GetComponent<HorizontalSlider>();
            if (sliderInstance != null)
            {
                sliderInstance.can_click = false;
                if(sliderInstance.nextSlider != null)
                {
                    sliderInstance.nextSlider.is_active = true;
                }
             
                hitScore.text = "MISS";
                smileys.ActivateSmiley(Smiley.Angry);
            }
        }
    }

}
