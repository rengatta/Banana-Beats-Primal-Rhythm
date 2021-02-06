using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InactiveSliderRegion : MonoBehaviour
{
    public enum SliderRegion {
        Left,
        Right
    }

    public SliderRegion sliderRegion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalHelper.sliderLayer)
        {
            SliderInterface sliderInterface = collision.gameObject.GetComponent<SliderInterface>();


            if (sliderInterface != null && sliderInterface.can_destroy && sliderInterface.sliderType == SliderType.RightSlider && sliderRegion == SliderRegion.Right)
            {
              
                
                sliderInterface.can_click = false;
                sliderInterface.is_active = false;
                if (sliderInterface.nextSlider != null)
                {
                    sliderInterface.nextSlider.is_active = true;
                }

                GlobalHelper.global.scoreManager.combo = 0;
                GlobalHelper.global.hitScoreText.text = "FAIL";
                GlobalHelper.global.smileys.ActivateSmiley(Smiley.Angry);

                sliderInterface.Darken();

               // Destroy(collision.gameObject);
            }
            else if (sliderInterface != null && sliderInterface.can_destroy && sliderInterface.sliderType == SliderType.LeftSlider && sliderRegion == SliderRegion.Left)
            {

                sliderInterface.can_click = false;
                sliderInterface.is_active = false;
                if (sliderInterface.nextSlider != null)
                {
                    sliderInterface.nextSlider.is_active = true;
                }

                GlobalHelper.global.scoreManager.combo = 0;
                GlobalHelper.global.hitScoreText.text = "FAIL";
                GlobalHelper.global.smileys.ActivateSmiley(Smiley.Angry);
                sliderInterface.Darken();

                //  Destroy(collision.gameObject);
            }




        }
     




    }

}
