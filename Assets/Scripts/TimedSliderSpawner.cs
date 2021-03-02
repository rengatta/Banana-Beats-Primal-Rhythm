using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSliderSpawner : MonoBehaviour
{

    public enum LevelSliderType
    {
        RightSlider,
        LeftSlider,
        RightHoldSLider,
        LeftHoldSlider
    }

    [System.Serializable]
    public class LevelData {
       

        public List<float> spawnTime;
        public List<LevelSliderType> sliderSpawns;

        public string audoClipPath;

    }


    public GameObject leftSliderPrefab;
    public Transform sliderSpawnPosition;

    public float sliderSpeed = 10f;


    IEnumerator Spawn() {
        while(true) {
            yield return new WaitForSeconds(2);
            RegularSlider sliderScript = Instantiate(leftSliderPrefab).GetComponent<RegularSlider>();
            sliderScript.transform.position = sliderSpawnPosition.position;
            sliderScript.horizontal_speed = sliderSpeed;


        }


    }

    void Start()
    {
        //StartCoroutine(Spawn());
    }


    void Update()
    {
        
    }
}
