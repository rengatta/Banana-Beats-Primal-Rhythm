using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
//controls the option menu logic
//retrieves and sets data for the options in PlayerPrefs
public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public UnityEvent volumeChangeEvent = new UnityEvent();
    public UnityEvent muteToggleChangeEvent = new UnityEvent();
    public Toggle muteToggle;
    public Dropdown resolutionDropdown;
    List<Resolution> resolutions;

    public GameObject androidDisableRoot;

    public UniversalRenderPipelineAsset urpa;

    public Toggle postProcessingToggle;

    void OnEnable() {
        InitializeOptions();
    }



    public void InitializeOptions()
    {

        if (Application.platform == RuntimePlatform.Android || GameState.androidMode == true)
        {
            androidDisableRoot.SetActive(false);
        }
        else
        {


            if (resolutionDropdown.options.Count == 0 && !Application.isEditor)
            
            {


                resolutions = GameState.GetResolutions();

                List<string> m_DropOptions = new List<string>();

                foreach (var res in resolutions)
                {
                    m_DropOptions.Add(res.width.ToString() + "x" + res.height.ToString());
                }

                resolutionDropdown.AddOptions(m_DropOptions);
            }


            int defaultDropdown = PlayerPrefs.GetInt("Resolution", 0);
            resolutionDropdown.value = defaultDropdown;

        }



        if (PlayerPrefs.GetInt("PostProcessing", 1) == 1) {
            postProcessingToggle.isOn = true;
        } else {
            postProcessingToggle.isOn = false;
        }


        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);
        volumeChangeEvent.Invoke();
       

     
        int muteToggled = PlayerPrefs.GetInt("MuteToggled", 0);
        if(muteToggled == 1) {
            muteToggle.isOn = true;
        } else {
            muteToggle.isOn = false;
        }
        muteToggleChangeEvent.Invoke();
        
    }

    public void SetResolution() {

 
        int fullScreenSet = PlayerPrefs.GetInt("Fullscreen", 1);
        int index = resolutionDropdown.value;

        PlayerPrefs.SetInt("Resolution", index);
        if (fullScreenSet == 0)
        {
            Screen.SetResolution(resolutions[index].width, resolutions[index].height, false);
        }
        else
        {
            Screen.SetResolution(resolutions[index].width, resolutions[index].height, true);
        }


    }

    public void SetFullScreen() {
        PlayerPrefs.SetInt("Fullscreen", 1);
        PlayerPrefs.SetInt("Windowed", 0);
        Screen.fullScreen = true;
    }

    public void SetWindowed() {
        PlayerPrefs.SetInt("Fullscreen", 0);
        PlayerPrefs.SetInt("Windowed", 1);
        Screen.fullScreen = false;
    }


    public void SliderSetVolume() {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        volumeChangeEvent.Invoke();
    }

    public void MuteToggleChanged()
    {
        int muteToggled = muteToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("MuteToggled", muteToggled);
        muteToggleChangeEvent.Invoke();
    }


    public void PostProcessingToggled() {

        if (!postProcessingToggle.isOn)
        {
            Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = false;
            urpa.supportsHDR = false;
            PlayerPrefs.SetInt("PostProcessing", 0);
        } else {
            Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            urpa.supportsHDR = true;
            PlayerPrefs.SetInt("PostProcessing", 1);
        }
    } 

}
