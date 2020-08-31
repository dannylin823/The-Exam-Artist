using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class SceneHandler : MonoBehaviour
{

    private SteamVR_LaserPointer laserPointer;
    public Settings setting;
    public GameObject SkillsOverlay;
    private AudioSource scribble;

    void Start()
    {
        setting = null;
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
        string hand = (setting != null) ? setting.getHand() : "LeftHand";

        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        GameObject SteamVRObjects = player.transform.Find("SteamVRObjects").gameObject;
        GameObject Hand = SteamVRObjects.transform.Find(hand).gameObject;

        Hand.transform.Find("Pencil").gameObject.SetActive(true);

        laserPointer = Hand.GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        if (hand == "LeftHand")
        {
            GameObject.Find("PlayerTable").transform.Find("BottleLeft").gameObject.SetActive(true);
            GameObject.Find("PlayerTable").transform.Find("BottleRight").gameObject.SetActive(false);
            GameObject.Find("Projectile").transform.Find("EraserLeft").gameObject.SetActive(false);
            GameObject.Find("Projectile").transform.Find("EraserRight").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("PlayerTable").transform.Find("BottleLeft").gameObject.SetActive(false);
            GameObject.Find("PlayerTable").transform.Find("BottleRight").gameObject.SetActive(true);
            GameObject.Find("Projectile").transform.Find("EraserLeft").gameObject.SetActive(true);
            GameObject.Find("Projectile").transform.Find("EraserRight").gameObject.SetActive(false);
            Vector3 position = SkillsOverlay.transform.localPosition;
            position.z += 1.526f;
            SkillsOverlay.transform.localPosition = position;
        }
        scribble = GameObject.FindGameObjectWithTag("TestSound").GetComponent<AudioSource>();
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            if(b.tag != "MainChoiceSelected")
            {
                if (b.tag == "MainChoice")
                {
                    scribble.Play();
                }
                else
                {
                    if (setting != null)
                    {
                        setting.click.Play();
                    }
                }
                if (b.enabled)
                {
                    b.onClick.Invoke();
                }
            }
        }

    }
    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            laserPointer.thickness = 0.002f;
            Button b = e.target.gameObject.GetComponent<Button>();
            if(b.tag != "MainChoiceSelected")
            {
                ColorBlock cb = b.colors;
                cb.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                b.colors = cb;
            }
        }
        if (e.target.tag == "MainTestPaper")
        {
            laserPointer.thickness = 0.002f;
        }
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            laserPointer.thickness = 0.0f;
            Button b = e.target.gameObject.GetComponent<Button>();
            if (b.tag != "MainChoiceSelected")
            {
                ColorBlock cb = b.colors;
                cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                b.colors = cb;
            }
        }
        if (e.target.tag == "MainTestPaper")
        {
            laserPointer.thickness = 0.0f;
        }
    }
}
