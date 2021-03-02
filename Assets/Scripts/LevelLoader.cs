using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public SliderManager sliderManager;

    public bool debugging = true;
    public string debugLevelName;

    public void InitializeLevel(string levelName) {
        LoadLevel(levelName);
        sliderManager.PlayLevel();
        GlobalHelper.global.audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(debugging) {
            InitializeLevel(debugLevelName);

        }
    }

    public void LoadLevel(string levelName)
    {
        //string path = Application.dataPath + "\\LevelSaves\\" + levelName;
        string path = Application.dataPath + "\\LevelSaves\\" + levelName;

        if (!File.Exists(path))
        {
            Debug.Log("FILE DOES NOT EXIST.");
            return;
        }
        else
        {
            string readText = File.ReadAllText(path);
            GlobalHelper.global.currentLevel = JsonUtility.FromJson<LevelData>(readText);
            GlobalHelper.global.currentAudioClip = Resources.Load<AudioClip>("Audio\\" + GlobalHelper.global.currentLevel.audioClipName);
            GlobalHelper.global.audioSource.clip = GlobalHelper.global.currentAudioClip;

        }


    }

}
