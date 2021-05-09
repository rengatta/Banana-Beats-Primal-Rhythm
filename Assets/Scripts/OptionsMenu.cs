using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public UnityEvent volumeChangeEvent = new UnityEvent();
    public UnityEvent muteToggleChangeEvent = new UnityEvent();
    public Toggle muteToggle;
    public Dropdown resolutionDropdown;
    List<Resolution> resolutions;

    public GameObject androidDisableRoot;

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


            if (resolutionDropdown.options.Count == 0)
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


}
