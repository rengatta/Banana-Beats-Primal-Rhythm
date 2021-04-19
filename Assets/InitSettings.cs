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

        switch (PlayerPrefs.GetInt("Resolution", 0))
        {
            case 0:
                if (fullScreenSet == 0)
                {
                    Screen.SetResolution(1280, 720, false);
                }
                else
                {
                    Screen.SetResolution(1280, 720, true);
                }
                break;
            case 1:
                break;
            default:
                break;
        }
        SceneToSceneData.gameOptionsInit = true;
    }


    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
