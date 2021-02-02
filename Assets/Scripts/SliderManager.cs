using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SliderManager : MonoBehaviour
{
    public GameObject sliderPrefab;

    public float test_spawn_rate = 1f;

    public float test_slider_speed = 7f;

    public Transform slider_spawn_area;

    public TextMeshProUGUI hitScoreText;

    public Smileys smileys;

    IEnumerator TestSpawner() {
        //WaitForSeconds spawnWfs = new WaitForSeconds(test_spawn_rate);
        HorizontalSlider previousSliderScript = null;
        while(true) {

            GameObject sliderInstance = Instantiate(sliderPrefab);
            sliderInstance.transform.position = slider_spawn_area.position;

            HorizontalSlider sliderScript = sliderInstance.GetComponent<HorizontalSlider>();
            sliderScript.hitScoreText = this.hitScoreText;
            sliderScript.horizontal_speed = test_slider_speed;
            sliderScript.smileys = this.smileys;

            if(previousSliderScript == null) {
                sliderScript.is_active = true;
            } else {
                previousSliderScript.nextSlider = sliderScript;
            }
            previousSliderScript = sliderScript;

            yield return new WaitForSeconds(test_spawn_rate);
        }

    }

    void Start()
    {
        StartCoroutine(TestSpawner());
    }


}
