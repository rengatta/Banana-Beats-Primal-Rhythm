using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Utilities;
using System.IO;

public class LevelSelectLevel : MonoBehaviour
{
    public TextMeshProUGUI songNameText;
    public string songName = "";
    public TextMeshProUGUI yourBestScore;
    public TextMeshProUGUI yourBestGrade;
    public TextMeshProUGUI yourBestCombo;
    public TextMeshProUGUI yourBestAccuracy;
    public string levelFilename = "";
    public SceneField songScene;
    private void OnValidate()
    {
        songNameText.text = songName;
    }

    private void Start()
    {
        FillPerformanceData();
    }
    void FillPerformanceData() {

        //string path = Application.dataPath + "\\LevelSaves\\" + levelName;
        string path = Application.dataPath + "\\PlayerPerformanceData\\" + levelFilename + ".playerdata";

        if (!File.Exists(path))
        {
            path = Application.dataPath + "\\PlayerPerformanceData\\nolevel.playerdata";
        }


        string readText = File.ReadAllText(path);

        PerformanceSaveData performanceData = JsonUtility.FromJson<PerformanceSaveData>(readText);

        yourBestScore.text = "Best Score: " + (int)performanceData.bestScore; 
        yourBestGrade.text = "Best Grade: " + performanceData.bestGrade;
        yourBestCombo.text = "Best Combo: " + performanceData.bestCombo;
        yourBestAccuracy.text = "Best Accuracy: " + System.Math.Round(performanceData.bestAccuracy, 2) + "%";
    }


    public void PlayButtonClicked() {
        SceneToSceneData.nextLevelName = levelFilename;
        SceneManager.LoadScene(songScene);
    }


}
