using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuObject;

    public AudioMixer musicAM;
    public AudioMixer effectsAM;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    public Toggle fullScreenToggle;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;

    Resolution[] resolutions;
    bool paused=false;
    // Start is called before the first frame update
    void Start()
    {
        GetRes();
        if (!PlayerPrefs.HasKey("MasterVolume")) NewData();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        paused=!paused;
        menuObject.SetActive(paused);
        Time.timeScale = (paused) ? 0:1;
        if (FreeFlyCamera.instance)
        {
            FreeFlyCamera.instance.enabled= !paused;
            Cursor.visible = paused;
            Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    void NewData()
    {
        PlayerPrefs.SetFloat("MasterVolume", .5f);
        PlayerPrefs.SetFloat("MusicVolume", 0);
        PlayerPrefs.SetFloat("EffectsVolume", 0);
        PlayerPrefs.SetInt("QualityIndex", 2);
        PlayerPrefs.SetInt("IsFullScreen", 1);
    }

    void GetRes()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currResIndex=0;

        List<string> options = new List<string>();
        for (int i=0; i<resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currResIndex=i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void LoadData()
    {
        float f;
        int i;

        f = PlayerPrefs.GetFloat("MasterVolume");
        AudioListener.volume = f;
        masterSlider.value = f;

        f = PlayerPrefs.GetFloat("MusicVolume");
        musicAM.SetFloat("Volume", f);
        musicSlider.value = f;

        f = PlayerPrefs.GetFloat("EffectsVolume");
        effectsAM.SetFloat("Volume", f);
        effectsSlider.value = f;

        i = PlayerPrefs.GetInt("QualityIndex");
        QualitySettings.SetQualityLevel(i);
        qualityDropdown.value = i;
        qualityDropdown.RefreshShownValue();

        i = PlayerPrefs.GetInt("IsFullScreen");
        bool b = i==1;
        fullScreenToggle.isOn = b;
        Screen.fullScreen = b;


    }

    public void SetMasterVolume(float f)
    {
        AudioListener.volume = f;
        PlayerPrefs.SetFloat("MasterVolume", f);
    }

    public void SetMusicVolume(float f)
    {
        musicAM.SetFloat("Volume", f);
        PlayerPrefs.SetFloat("MusicVolume", f);
    }

    public void SetEffectsVolume(float f)
    {
        effectsAM.SetFloat("Volume", f);
        PlayerPrefs.SetFloat("EffectsVolume", f);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        int f = (isFullScreen) ? 1:0;
        PlayerPrefs.SetInt("IsFullScreen", f);
    }
}
