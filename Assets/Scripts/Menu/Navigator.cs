using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;
public class Navigator : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointerL;
    private SteamVR_LaserPointer laserPointerR;
    private Settings setting;
    private Volume volume;
    private LevelSetting levelsetting;
    private ScoreCalculate report;
    public GameObject LoadSceneHandler;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        GameObject SteamVRObjects = player.transform.Find("SteamVRObjects").gameObject;
        GameObject LeftHand = SteamVRObjects.transform.Find("LeftHand").gameObject;
        GameObject RightHand = SteamVRObjects.transform.Find("RightHand").gameObject;

        laserPointerL = LeftHand.GetComponent<SteamVR_LaserPointer>();
        laserPointerR = RightHand.GetComponent<SteamVR_LaserPointer>();

        laserPointerL.thickness = 0.002f;
        laserPointerR.thickness = 0.002f;

        laserPointerL.PointerIn += PointerInside;
        laserPointerL.PointerOut += PointerOutside;
        laserPointerL.PointerClick += PointerClick;
        laserPointerR.PointerIn += PointerInside;
        laserPointerR.PointerOut += PointerOutside;
        laserPointerR.PointerClick += PointerClick;

        setting = null;
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
        if (GameObject.Find("VolumeHandler"))
        {
            volume = GameObject.Find("VolumeHandler").GetComponent<Volume>();
        }

        if (GameObject.Find("ScoreCalculate"))
        {
            report = GameObject.Find("ScoreCalculate").GetComponent<ScoreCalculate>();
        }

    }
    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {

            Button b = e.target.gameObject.GetComponent<Button>();
            b.onClick.Invoke();
            if (setting != null)
            {
                setting.click.Play();
            }
        }
    }
    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.13f, 0.22f, 0.2f, 1.0f);
            if (e.target.tag == "InstructionButton")
            {
                cb.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            b.colors = cb;
        }
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.13f, 0.22f, 0.2f, 0.0f);
            if (e.target.tag == "InstructionButton")
            {
                cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            b.colors = cb;
        }
    }

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.black, 2.0f);
    }
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.clear, 2.0f);
    }

    public void Play()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("MainMenu").gameObject.SetActive(false);
        blackboard.transform.Find("HandSelect").gameObject.SetActive(true);
    }
    public void Options()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("MainMenu").gameObject.SetActive(false);
        blackboard.transform.Find("OptionMenu").gameObject.SetActive(true);
    }
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void Return()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("OptionMenu").gameObject.SetActive(false);
        blackboard.transform.Find("MainMenu").gameObject.SetActive(true);
    }
    public void Left()
    {
        if (setting != null) { setting.setHand("LeftHand"); }
        if (SteamVR.active)
        {
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            Initiate.Fade("Level 1", Color.black, 0.5f);
        }
    }
    public void Right()
    {
        if (setting != null) { setting.setHand("RightHand"); }
        if (SteamVR.active)
        {
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            Initiate.Fade("Level 1", Color.black, 0.5f);
        }
    }
    public void Back()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("HandSelect").gameObject.SetActive(false);
        blackboard.transform.Find("MainMenu").gameObject.SetActive(true);
    }
    public void TryAgain()
    {
        if (GameObject.Find("LevelSetting"))
        {
            LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
            setting.resetTemp();
            Destroy(GameObject.Find("LevelSetting"));
        }
        if (SteamVR.active)
        {
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            Initiate.Fade("Level 1", Color.black, 0.5f);
        }
    }
    public void Continue()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("ScoreMenu").gameObject.SetActive(false);
        blackboard.transform.Find("DecideMenu").gameObject.SetActive(true);
    }
    public void ShowReport()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("ScoreReport").gameObject.SetActive(true);
        blackboard.transform.Find("ScoreMenu").gameObject.SetActive(false);
        blackboard.transform.Find("Props").gameObject.SetActive(false);
    }
    public void DecideBack()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("ScoreReport").gameObject.SetActive(false);
        blackboard.transform.Find("ScoreMenu").gameObject.SetActive(true);
        blackboard.transform.Find("Props").gameObject.SetActive(true);
    }
    public void PrevReport()
    {
        report.prevReport();
    }
    public void NextReport()
    {
        report.nextReport();
    }
}
