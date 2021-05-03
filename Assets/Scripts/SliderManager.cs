using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

//hitscore for slider hit accuracy
enum HitScore
{
    Perfect,
    Good,
    Miss,
    Fail
}

//used for differentiating sliders in in the level
[System.Serializable]
public enum LevelSliderType
{
    RightSlider,
    LeftSlider,
    RightHoldSlider,
    LeftHoldSlider
}


//level data for holding slider spawn information, serializable in json format
[System.Serializable]
public class HoldSliderDictionary : SerializableDictionary<int, double> { }

[System.Serializable]
public class LevelData
{

    public string levelName;
    public List<double> hitTimes;
    public List<LevelSliderType> sliderSpawns;
    public HoldSliderDictionary holdSliderEndTimes;
    public string audioClipName;
    public double sliderSpeed;
    public float regularSliderScore = 10f;
    public float holdSliderScore = 5f;
    public float holdSliderHoldScore = 1f;

    LevelData() {
        hitTimes = new List<double>();
        sliderSpawns = new List<LevelSliderType>();
        holdSliderEndTimes = new HoldSliderDictionary();
        levelName = "defaultlevelname";
        audioClipName = "";
        sliderSpeed = 6f;

    }

    public int AddSlider(LevelSliderType sliderSpawn, float hitTime) {

        int findIndex = hitTimes.FindIndex(x => x == hitTime);

        sliderSpawns.Add(sliderSpawn);

        hitTimes.Add(System.Math.Round(hitTime, 4));
        //returns the index of the latest added slider
        return sliderSpawns.Count - 1;
    }

    public void AddHoldSliderEnd(int index, float endTime) {

        holdSliderEndTimes.Add(index, System.Math.Round(endTime, 4));
    }

    public void ClearData() {
        hitTimes.Clear();
        sliderSpawns.Clear();
        holdSliderEndTimes.Clear();

    }
}


//spawns slider for each level based on the current level data in global
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
    public bool testing = false;
    public Transform triangleCenter;
    public List<GameObject> activeSliders = new List<GameObject>();

    public float regularSliderScore = 10f;
    public float holdSliderScore = 5f;
    public float holdSliderHoldScore = 0.1f;
    public float perfectMultiplier = 1.5f;
    float totalScore = 0f;
    int totalCombo = 0;
    public AudioSource audioSource;


    public TextMeshProUGUI testText1;
    public TextMeshProUGUI testText2;


    float currentSongTime = 0.0f;



    class SpawnData{
        public float spawnTime = 0.0f;
        public float holdSliderEndPosition = 0.0f;
        public LevelSliderType sliderType = LevelSliderType.LeftSlider;
        public SpawnData(float spawnTime,LevelSliderType sliderType, float holdSliderEndPosition = 0.0f) {
            this.spawnTime = spawnTime;

            this.sliderType = sliderType;
            this.holdSliderEndPosition = holdSliderEndPosition;
        }
    }



    public void DestroyActiveSliders() {
        for(int i=0; i < activeSliders.Count; i++) {
            if(activeSliders[i] != null ) 
                Destroy(activeSliders[i]);
        }
        activeSliders.Clear();
    }


    void RegularSpawn(float speed, List<SpawnData> spawnData)
    {

        float spawnPosition;

        switch (spawnData[spawnData.Count - 1].sliderType)
        {
            case LevelSliderType.LeftSlider:
                spawnPosition = (spawnData[spawnData.Count - 1].spawnTime - currentSongTime) * speed;
                SpawnLeft(speed, -spawnPosition);
                spawnData.RemoveAt(spawnData.Count - 1);
                break;
            case LevelSliderType.RightSlider:
                spawnPosition = (spawnData[spawnData.Count - 1].spawnTime - currentSongTime) * speed;
                SpawnRight(speed, spawnPosition);
                spawnData.RemoveAt(spawnData.Count - 1);
                break;
            case LevelSliderType.LeftHoldSlider:
                spawnPosition = (spawnData[spawnData.Count - 1].spawnTime - currentSongTime) * speed;
                SpawnLeftHold(speed, -spawnPosition, spawnData[spawnData.Count - 1].holdSliderEndPosition);
                spawnData.RemoveAt(spawnData.Count - 1);
                break;
            case LevelSliderType.RightHoldSlider:
                spawnPosition = (spawnData[spawnData.Count - 1].spawnTime - currentSongTime) * speed;
                SpawnRightHold(speed, spawnPosition, spawnData[spawnData.Count - 1].holdSliderEndPosition);
                spawnData.RemoveAt(spawnData.Count - 1);
                break;

        }


    }


    IEnumerator TimedSpawner(float speed, List<SpawnData> spawnData,  float preTime, float songTime) {

        float spawnBuffer = 5.0f;
        GlobalHelper.global.audioSource.time = songTime;

        bool alreadyPlayed = false;
        double startDspTime = AudioSettings.dspTime;

        while (spawnData.Count > 0) {
 
            currentSongTime = (float)(AudioSettings.dspTime - startDspTime) + songTime;
            if (currentSongTime >= preTime && alreadyPlayed != true) {
                GlobalHelper.global.audioSource.Play();
                alreadyPlayed = true;
            }

            while (currentSongTime >= spawnData[spawnData.Count-1].spawnTime - spawnBuffer) {
                currentSongTime = (float)(AudioSettings.dspTime - startDspTime) + songTime;
                RegularSpawn(speed, spawnData);
                if (spawnData.Count <= 0) break;

            }

            yield return null;

        }

    }

    public void PlayLevelRegular(float preTime = 3.0f) {
        LevelData currentLevel = GlobalHelper.global.currentLevel;
        double speed = currentLevel.sliderSpeed;
        double spawnTime;
        double endSliderLength;


        totalScore = 0f;
        totalCombo = 0;
        List<SpawnData> spawnData = new List<SpawnData>(2000);
 
        for (int i=0; i < currentLevel.hitTimes.Count; i++) {
            switch (currentLevel.sliderSpawns[i])
            {
                case LevelSliderType.LeftSlider:
                    spawnTime = (currentLevel.hitTimes[i] + preTime);
                    spawnData.Add(new SpawnData((float)spawnTime,LevelSliderType.LeftSlider));
                    totalScore += SceneToSceneData.sliderScore * SceneToSceneData.perfectMultiplier;
                    totalCombo += 1;
                    break;
                case LevelSliderType.RightSlider:
                    spawnTime = (currentLevel.hitTimes[i] + preTime);
                    spawnData.Add(new SpawnData((float)spawnTime,LevelSliderType.RightSlider));
                    totalScore += SceneToSceneData.sliderScore * SceneToSceneData.perfectMultiplier;
                    totalCombo += 1;
                    break;
                case LevelSliderType.LeftHoldSlider:
                    spawnTime = (currentLevel.hitTimes[i] + preTime);
                    endSliderLength = (currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]);
                    spawnData.Add(new SpawnData((float)spawnTime,  LevelSliderType.LeftHoldSlider, (float)endSliderLength));
                    totalScore += SceneToSceneData.holdSliderScore * SceneToSceneData.perfectMultiplier * 2;
                    totalScore += SceneToSceneData.holdSliderHoldScore * (float)endSliderLength;
                    totalCombo += 2;
                    break;
                case LevelSliderType.RightHoldSlider:
                    spawnTime = (currentLevel.hitTimes[i] + preTime);
                    endSliderLength = (currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]);
                    spawnData.Add(new SpawnData((float)spawnTime,  LevelSliderType.RightHoldSlider, (float)endSliderLength));
                    totalScore += SceneToSceneData.holdSliderScore * SceneToSceneData.perfectMultiplier * 2;
                    totalScore += SceneToSceneData.holdSliderHoldScore * (float)endSliderLength;
                    totalCombo += 2;
                    break;
                default:
                    break;
            }
        }

        spawnData = spawnData.OrderByDescending(data => data.spawnTime).ToList();

        SceneToSceneData.totalTries = 0;
        SceneToSceneData.maxPossibleScore = this.totalScore;
        SceneToSceneData.maxPossibleCombo = this.totalCombo;
        StopAllCoroutines();
        StartCoroutine(TimedSpawner((float)speed, spawnData, preTime, 0f));
    }

    public void PlayLevelAtTime(float time)
    {

        LevelData currentLevel = GlobalHelper.global.currentLevel;
        double speed = currentLevel.sliderSpeed;
        double spawnTime;
        double endSliderLength;
        float offsetTime = time - 2f;
 
        totalScore = 0f;
        totalCombo = 0;
        List<SpawnData> spawnData = new List<SpawnData>(2000);


        for (int i = 0; i < currentLevel.hitTimes.Count; i++)
        {
            switch (currentLevel.sliderSpawns[i])
            {
                case LevelSliderType.LeftSlider:
                    if (currentLevel.hitTimes[i] < offsetTime) break;
                    spawnTime = (currentLevel.hitTimes[i]);
                    spawnData.Add(new SpawnData((float)spawnTime,  LevelSliderType.LeftSlider));
                    totalScore += SceneToSceneData.sliderScore * SceneToSceneData.perfectMultiplier;
                    totalCombo += 1;
                    break;
                case LevelSliderType.RightSlider:
                    if (currentLevel.hitTimes[i] < offsetTime) break;
                    spawnTime = (currentLevel.hitTimes[i]);
                    spawnData.Add(new SpawnData((float)spawnTime,  LevelSliderType.RightSlider));
                    totalScore += SceneToSceneData.sliderScore * SceneToSceneData.perfectMultiplier;
                    totalCombo += 1;
                    break;
                case LevelSliderType.LeftHoldSlider:
                    if (currentLevel.hitTimes[i] < offsetTime) break;
                    spawnTime = (currentLevel.hitTimes[i]);
                    endSliderLength = (currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]);
                    spawnData.Add(new SpawnData((float)spawnTime,  LevelSliderType.LeftHoldSlider, (float)endSliderLength));
                    totalScore += SceneToSceneData.holdSliderScore * SceneToSceneData.perfectMultiplier * 2;
                    totalScore += SceneToSceneData.holdSliderHoldScore * (float)endSliderLength;
                    totalCombo += 2;
                    break;
                case LevelSliderType.RightHoldSlider:
                    if (currentLevel.hitTimes[i] < offsetTime) break;
                    spawnTime = (currentLevel.hitTimes[i]);
                    endSliderLength = (currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]);
                    spawnData.Add(new SpawnData((float)spawnTime,  LevelSliderType.RightHoldSlider, (float)endSliderLength));
                    totalScore += SceneToSceneData.holdSliderScore * SceneToSceneData.perfectMultiplier * 2;
                    totalScore += SceneToSceneData.holdSliderHoldScore * (float)endSliderLength;
                    totalCombo += 2;
                    break;
                default:
                    break;
            }
        }


        spawnData = spawnData.OrderByDescending(data => data.spawnTime).ToList();

        SceneToSceneData.totalTries = 0;
        SceneToSceneData.maxPossibleScore = this.totalScore;
        SceneToSceneData.maxPossibleCombo = this.totalCombo;


        StopAllCoroutines();
        StartCoroutine(TimedSpawner((float)speed, spawnData, 0f, time));

    }


 


    void SpawnRight(float speed, float positionx) {
        GameObject sliderInstance = Instantiate(rightSliderPrefab);
        sliderInstance.transform.position = new Vector3(positionx, sliderSpawnArea.position.y, 0f);

        SliderInterface sliderScript = sliderInstance.GetComponent<SliderInterface>();
        sliderScript.horizontal_speed = speed;
        sliderScript.score = regularSliderScore;

        activeSliders.Add(sliderInstance);

        totalScore += regularSliderScore * perfectMultiplier;
        totalCombo += 1;
    }

    void SpawnRightHold(float speed, float positionx, float length)
    {
        GameObject sliderInstance = Instantiate(rightHoldSliderPrefab);
        sliderInstance.transform.position = new Vector3(positionx, sliderSpawnArea.position.y, 0f);
        HoldSlider holdSliderScript = sliderInstance.GetComponent<HoldSlider>();
        holdSliderScript.Initialize(length);


        SliderInterface sliderScript = sliderInstance.GetComponent<SliderInterface>();
        sliderScript.score = holdSliderScore;
        sliderScript.horizontal_speed = speed;

        activeSliders.Add(sliderInstance);
        totalScore += holdSliderScore * perfectMultiplier * 2.0f;
        totalScore += holdSliderHoldScore * length;
        totalCombo += 2;
    }

    void SpawnLeftHold(float speed, float positionx, float length)
    {
        GameObject sliderInstance = Instantiate(leftHoldSliderPrefab);
        sliderInstance.transform.position = new Vector3(positionx, leftSliderSpawnArea.position.y, 0f);

        HoldSlider holdSliderScript = sliderInstance.GetComponent<HoldSlider>();
        holdSliderScript.Initialize(length);

        SliderInterface sliderScript = sliderInstance.GetComponent<SliderInterface>();
        sliderScript.score = holdSliderScore;
        sliderScript.horizontal_speed = speed;


        activeSliders.Add(sliderInstance);
        totalScore += holdSliderScore * perfectMultiplier * 2.0f;
        totalScore += holdSliderHoldScore * length;
        totalCombo += 2;

    }

    void SpawnLeft(float speed, float positionx) {
        GameObject sliderInstance = Instantiate(leftSliderPrefab);
        sliderInstance.transform.position = new Vector3(positionx, leftSliderSpawnArea.position.y, 0f);

        SliderInterface sliderScript = sliderInstance.GetComponent<SliderInterface>();
        sliderScript.score = holdSliderScore;
        sliderScript.horizontal_speed = speed;

        activeSliders.Add(sliderInstance);
        totalScore += regularSliderScore * perfectMultiplier;
        totalCombo += 1;
    }



}
