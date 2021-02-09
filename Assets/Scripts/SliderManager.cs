using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum HitScore
{
    Perfect,
    Good,
    Miss,
    Fail
}



public class SliderManager : MonoBehaviour
{


    public GameObject rightSliderPrefab;
    public GameObject leftSliderPrefab;
    public GameObject leftHoldSliderPrefab;
    public GameObject rightHoldSliderPrefab;


    public float test_spawn_rate = 1f;
    public float test_slider_speed = 7f;

    public Transform sliderSpawnArea;
    public Transform leftSliderSpawnArea;

    IEnumerator TestSpawner() {
        //WaitForSeconds spawnWfs = new WaitForSeconds(test_spawn_rate);
        SliderInterface previousSliderScriptRight = null;
        SliderInterface previousSliderScriptLeft = null;
        while (true) {

            {
                GameObject sliderInstance = Instantiate(rightHoldSliderPrefab);
                sliderInstance.transform.position = sliderSpawnArea.position;

                SliderInterface sliderScript = sliderInstance.GetComponent<SliderInterface>();
                sliderScript.horizontal_speed = test_slider_speed;

                if (previousSliderScriptRight == null || previousSliderScriptRight.Equals(null))
                {
                    sliderScript.is_active = true;
                }
                else if (previousSliderScriptRight != null && !previousSliderScriptRight.is_active)
                {
                    sliderScript.is_active = true;

                }
                else
                {
                    previousSliderScriptRight.nextSlider = sliderScript;
                }


                previousSliderScriptRight = sliderScript;

                yield return new WaitForSeconds(test_spawn_rate);
            }

       
            {

                GameObject sliderInstance = Instantiate(leftHoldSliderPrefab);
                sliderInstance.transform.position = leftSliderSpawnArea.position;

                SliderInterface sliderScript = sliderInstance.GetComponent<SliderInterface>();
                sliderScript.horizontal_speed = test_slider_speed;


                if (previousSliderScriptLeft == null || previousSliderScriptLeft.Equals(null))
                {
                    sliderScript.is_active = true;
                }
                else if (previousSliderScriptLeft != null && !previousSliderScriptLeft.is_active)
                {
                    sliderScript.is_active = true;

                }
                else
                {
                    previousSliderScriptLeft.nextSlider = sliderScript;
                }

                previousSliderScriptLeft = sliderScript;


                yield return new WaitForSeconds(test_spawn_rate);


            }

        }

    }

    void Start()
    {
        StartCoroutine(TestSpawner());
    }


}
