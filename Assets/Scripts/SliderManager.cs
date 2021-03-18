using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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



    public void DestroyActiveSliders() {
        for(int i=0; i < activeSliders.Count; i++) {
            if(activeSliders[i] != null ) 
                Destroy(activeSliders[i]);
        }
        activeSliders.Clear();
    }

    public void PlayLevelDebug(float debugStartTime = 0.0f) {
        float songStartTime = debugStartTime * audioSource.clip.length;
        PlayLevelAtTime(songStartTime);
        
    }

    public void PlayLevelRegular(float preTime = 3.0f) {
        LevelData currentLevel = GlobalHelper.global.currentLevel;
        double speed = currentLevel.sliderSpeed;
        double spawnPosition;
        totalScore = 0f;
        totalCombo = 0;
        for (int i=0; i < currentLevel.hitTimes.Count; i++) {
            switch (currentLevel.sliderSpawns[i])
            {
                case LevelSliderType.LeftSlider:
                    spawnPosition = (currentLevel.hitTimes[i] + preTime) * speed;
                    SpawnLeft((float)speed, -(float)spawnPosition);
                    break;
                case LevelSliderType.RightSlider:
                    spawnPosition = (currentLevel.hitTimes[i] + preTime) * speed;
                    SpawnRight((float)speed, (float)spawnPosition);
                    break;
                case LevelSliderType.LeftHoldSlider:
                    spawnPosition = (currentLevel.hitTimes[i] + preTime) * speed;
                    SpawnLeftHold((float)speed, -(float)spawnPosition, (float)(currentLevel.holdSliderEndTimes[i]- currentLevel.hitTimes[i]));
                    break;
                case LevelSliderType.RightHoldSlider:
                    spawnPosition = (currentLevel.hitTimes[i] + preTime) * speed;
                    SpawnRightHold((float)speed, (float)spawnPosition, (float)(currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]));
                    break;
                default:
                    break;
            }
        }
        SceneToSceneData.maxPossibleScore = this.totalScore;
        SceneToSceneData.maxPossibleCombo = this.totalCombo;
    }

    public void PlayLevelAtTime(float time)
    {
        LevelData currentLevel = GlobalHelper.global.currentLevel;
        double speed = currentLevel.sliderSpeed;
        double spawnPosition;
        float offsetTime = time - 2f;
        totalScore = 0f;
        totalCombo = 0;
        for (int i = 0; i < currentLevel.hitTimes.Count; i++)
        {
            switch (currentLevel.sliderSpawns[i])
            {
                case LevelSliderType.LeftSlider:
                    if (currentLevel.hitTimes[i] < offsetTime) break;
                    spawnPosition = currentLevel.hitTimes[i] * speed - time * speed;
                    SpawnLeft((float)speed, -(float)spawnPosition);
                    break;
                case LevelSliderType.RightSlider:
                    if (currentLevel.hitTimes[i]  < offsetTime) break;
                    spawnPosition = currentLevel.hitTimes[i]  * speed - time * speed;
                    SpawnRight((float)speed, (float)spawnPosition);
                    break;
                case LevelSliderType.LeftHoldSlider:
                    if (currentLevel.hitTimes[i] < offsetTime) break;
                    spawnPosition = currentLevel.hitTimes[i] * speed - time * speed;
                    SpawnLeftHold((float)speed, -(float)spawnPosition, (float)(currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]));
                    break;
                case LevelSliderType.RightHoldSlider:
                    if (currentLevel.hitTimes[i] < offsetTime) break;
                    spawnPosition = currentLevel.hitTimes[i] * speed - time * speed;
                    SpawnRightHold((float)speed, (float)spawnPosition, (float)(currentLevel.holdSliderEndTimes[i] - currentLevel.hitTimes[i]));
                    break;
                default:
                    break;
            }
        }
        SceneToSceneData.maxPossibleScore = this.totalScore;
        SceneToSceneData.maxPossibleCombo = this.totalCombo;

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
        holdSliderScript.holdScore = holdSliderHoldScore;
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
        holdSliderScript.holdScore = holdSliderHoldScore;
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
