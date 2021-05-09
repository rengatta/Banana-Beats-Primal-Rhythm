using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSettings : MonoBehaviour
{

    void Initialize()
    {
        if (SceneToSceneData.gameOptionsInit) return;

        if (PlayerPrefs.GetInt("Fullscreen", 1) == 1)
        {
            Screen.fullScreen = true;

        }
        
        if (PlayerPrefs.GetInt("Windowed", 0) == 1)
        {
            Screen.fullScreen = false;

        }

        int fullScreenSet = PlayerPrefs.GetInt("Fullscreen", 1);

        List<Resolution> resolutions = GameState.GetResolutions();

        int index = PlayerPrefs.GetInt("Resolution", 0);

        if (fullScreenSet == 0)
        {
            Screen.SetResolution(resolutions[index].width, resolutions[index].height, false);
        }
        else
        {
            Screen.SetResolution(resolutions[index].width, resolutions[index].height, true);
        }

        SceneToSceneData.gameOptionsInit = true;
    }


    void Start()
    {
        Initialize();
    }


    
}
