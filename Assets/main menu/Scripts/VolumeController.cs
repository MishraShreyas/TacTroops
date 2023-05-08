using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Text volumeText;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Volume")) PlayerPrefs.SetFloat("Volume", .5f);
        LoadVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumeSlider(float volume)
    {
        int v = (int)(volume*100);
        volumeText.text = v.ToString();

        AudioListener.volume = volumeSlider.value;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        AudioListener.volume = volumeSlider.value;
    }
}
