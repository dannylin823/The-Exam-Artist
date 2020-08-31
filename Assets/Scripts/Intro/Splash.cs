using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Splash : MonoBehaviour
{
    public Image usc, logo;
    public GameObject LoadSceneHandler;
    private bool usc_in = false, usc_out = false, logo_in = false, logo_out = false;
    private bool usc_out_s = false, logo_in_s = false, logo_out_s = false;
    private Light[] lights;

    void Start()
    {
        if (SteamVR.active)
        {
            StartCoroutine(FadeIn(usc, 2.0f));
        }
        else
        {
            lights = FindObjectsOfType(typeof(Light)) as Light[];
            foreach (Light light in lights)
            {
                light.intensity = 0;
            }
            LoadSceneHandler.SetActive(true);
        }
    }

    void Update()
    {
        if (!usc_out && usc_in && !usc_out_s)
        {
            StartCoroutine(FadeOut(usc, 2.0f));
            usc_out_s = true;
        }
        else if (!logo_in && usc_out && !logo_in_s)
        {
            StartCoroutine(FadeIn(logo, 2.0f));
            logo_in_s = true;
        }
        else if (!logo_out && logo_in && !logo_out_s)
        {
            StartCoroutine(FadeOut(logo, 2.0f));
            logo_out_s = true;
        }
        else if (logo_out)
        {
            LoadSceneHandler.SetActive(true);
        }
    }

    private IEnumerator FadeOut(Image i, float FadeTime)
    {
        while (i.color.a > 0)
        {
            i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, i.transform.localPosition.z - 0.2f);
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / FadeTime));
            yield return null;
        }
        if (i == usc) { usc_out = true; }
        if (i == logo) { logo_out = true; }
    }

    private IEnumerator FadeIn(Image i, float FadeTime)
    {
        while (i.color.a < 1)
        {
            i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, i.transform.localPosition.z - 0.2f);
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / FadeTime));
            yield return null;
        }
        if (i == usc) { usc_in = true; }
        if (i == logo) { logo_in = true; }
    }

}
