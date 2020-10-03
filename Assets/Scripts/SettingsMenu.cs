using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Michsky.UI.ModernUIPack;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer masterMixer;

    public TMP_Dropdown resDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int _resIndex)
    {
        Resolution res = resolutions[_resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float _volume)
    {
        masterMixer.SetFloat("volume", _volume);
    }

    public void SetQuality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }

    public void SetFullscreen(bool _isFullscreen)
    {
        Screen.fullScreen = _isFullscreen;
    }
}
