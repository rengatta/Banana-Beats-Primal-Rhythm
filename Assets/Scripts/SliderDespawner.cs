using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//region to the very left or right of the camera that destroys slider gameobjects
public class SliderDespawner : MonoBehaviour
{
    public SliderType sliderType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check for slider layer
        if(collision.gameObject.layer == GlobalHelper.sliderLayer) {

            SliderInterface sliderInterface = collision.gameObject.GetComponent<SliderInterface>();
            if (sliderInterface != null)
            {

                if (sliderType == SliderType.LeftSlider && sliderInterface.sliderType == SliderType.LeftSlider)
                {
                    Destroy(collision.gameObject);

                }
                else if (sliderType == SliderType.RightSlider && sliderInterface.sliderType == SliderType.RightSlider)
                {
                    Destroy(collision.gameObject);

                }
            }

        }
    }
}
