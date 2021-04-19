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

    void OnEnable() {
        InitializeOptions();
    }


    public void InitializeOptions()
    {
     
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

        //0 - 
        int fullScreenSet = PlayerPrefs.GetInt("Fullscreen", 1);

        switch (resolutionDropdown.value)
        {
            case 0:
                PlayerPrefs.SetInt("Resolution", 0);
                if(fullScreenSet == 0) {
                    Screen.SetResolution(1280, 720, false);
                } else {
                    Screen.SetResolution(1280, 720, true);
                }
                break;
            case 1:
                PlayerPrefs.SetInt("Resolution", 1);
                break;
            default:
                break;
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
