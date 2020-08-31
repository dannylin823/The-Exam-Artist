using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class VolumeSlider : MonoBehaviour
{
    public LinearMapping mapping;
    public RectTransform fill;
    public Text volumeText;
    public float volume;
    private Settings setting;
    private AudioSource sound;
    private float initial = 0.5f;

    void Start()
    {
        volumeText.text = ((int)volume).ToString();
        setting = GameObject.Find("Settings").GetComponent<Settings>();
        sound = GameObject.Find("Sound").GetComponent<AudioSource>();
    }

    public void ToneUp()
    {
        if (volume < 100)
        {
            volume++;
            //slider.value = volume / 100.0f;
            volumeText.text = ((int)volume).ToString();
            setting.setVolume((int)volume);
            sound.volume = initial * ((float)(setting.getVolume() - 50) / (float)50 + 1);
        }
    }

    public void ToneDown()
    {
        if (volume > 0)
        {
            volume--;
            //slider.value = volume / 100.0f;
            volumeText.text = ((int)volume).ToString();
            setting.setVolume((int)volume);
            sound.volume = initial * ((float)(setting.getVolume() - 50) / (float)50 + 1);
        }
    }
}
