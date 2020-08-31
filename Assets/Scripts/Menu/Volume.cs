using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider slider;
    public Text volumeText;
    public float volume;
    private Settings setting;
    private AudioSource sound;
    private float initial = 0.5f;

    void Start()
    {
        slider.value = volume / 100.0f;
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
