using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages settings for audiosources that are for the background and not sound effects
//stores and retrieves settings from PlayerPrefs and listens to an event in the Options menu whenever sound options are changed
[RequireComponent(typeof(AudioSource))]
public class BGMAudioHolder : MonoBehaviour
{

    AudioSource audioSource;
    public OptionsMenu optionsMenu;
    float startVolume;
    public void SetOptions() {
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 1.0f) * startVolume;

        int muteToggled = PlayerPrefs.GetInt("MuteToggled", 0);
        if (muteToggled == 1)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;

        if (optionsMenu != null)
        {
            optionsMenu.volumeChangeEvent.AddListener(SetOptions);
            optionsMenu.muteToggleChangeEvent.AddListener(SetOptions);
        }
        SetOptions();

    }
}
