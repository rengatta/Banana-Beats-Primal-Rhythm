using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
//used to retrieve options menu settings previously stored in PlayerPrefs and apply them to the game inside of the main menu scene
public class InitSettings : MonoBehaviour
{
    public UniversalRenderPipelineAsset urpa;

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

        if (PlayerPrefs.GetInt("PostProcessing", 1) == 1)
        {
            Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            urpa.supportsHDR = true;
        }
        else
        {
            Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = false;
            urpa.supportsHDR = false;
        }



        int fullScreenSet = PlayerPrefs.GetInt("Fullscreen", 1);

        if (!Application.isEditor)
        {
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
        }
        SceneToSceneData.gameOptionsInit = true;
    }


    void Start()
    {
        Initialize();
    }


    
}
