using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//region to the left or right of the white triangle that darkens sliders
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


            if (sliderInterface != null && sliderInterface.hit != true && sliderInterface.can_destroy && sliderInterface.sliderType == SliderType.RightSlider && sliderRegion == SliderRegion.Right)
            {

                   
                GlobalHelper.global.FailHit();

                sliderInterface.Darken();

               // Destroy(collision.gameObject);
            }
            else if (sliderInterface != null && sliderInterface.hit != true  &&  sliderInterface.can_destroy && sliderInterface.sliderType == SliderType.LeftSlider && sliderRegion == SliderRegion.Left)
            {

                GlobalHelper.global.FailHit();
                sliderInterface.Darken();

                //  Destroy(collision.gameObject);
            }


        }
     
    }

}
