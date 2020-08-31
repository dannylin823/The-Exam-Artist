using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.SceneManagement;

public class GodOfWashroomBehavior : MonoBehaviour
{
    public Image imgCoolDown;
    public Text textCoolDown, limitText;
    public GameObject timer;
    public GameObject testPaper;
    private AudioSource[] sound;
    public AudioClip[] washroomAudioClips = new AudioClip[7];
    private float coolDown = 14.0f;
    private float coolDownCounter = 14.0f;
    private bool used = false, enoughTime = true;
    private int limit = 5;
    private float duration = 2.0f;
    private LevelSetting setting;
    public GameObject LoadSceneHandler, Washroom, Level, projectile;
    private TimeFreezeBehavior tf;

    private void loadResources()
    {
        GameObject table = GameObject.Find("PlayerTable");
        GameObject SkillsOverlay = table.transform.Find("SkillsOverlay").gameObject;
        GameObject SkillCoolDown = SkillsOverlay.transform.Find("SkillCoolDown").gameObject;
        GameObject skill = SkillCoolDown.transform.Find("GodOfWashroom").gameObject;
        GameObject resources = skill.transform.Find("Image").gameObject;

        imgCoolDown = resources.transform.Find("CDImg").gameObject.GetComponent<Image>();
        textCoolDown = resources.transform.Find("CDText").gameObject.GetComponent<Text>();

        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    void Start()
    {
        loadResources();
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        imgCoolDown.fillAmount = 0.0f;
        textCoolDown.text = "";
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        if (setting.washroomed)
        {
            used = true;
        }
        limitText.text = limit.ToString();
    }

    void Update()
    {
        if (!tf.hold)
        {
            if (timer.GetComponent<Timer>().timeLeft < 60 && enoughTime || limit == 0)
            {
                imgCoolDown.fillAmount = 1.0f;
                imgCoolDown.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
                textCoolDown.text = "";
                enoughTime = false;
                //Debug.Log("Not enough time to go to washroom!");
            }
            else
            {
                if (coolDownCounter > 0 && used == true)
                {
                    coolDownCounter -= Time.deltaTime;
                    imgCoolDown.fillAmount = 1 - coolDownCounter / coolDown;
                    textCoolDown.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
                    //Debug.Log(coolDownCounter[0]);
                }
                else if (coolDownCounter <= 0 && used == true)
                {
                    coolDownCounter = coolDown;
                    textCoolDown.text = "";
                    imgCoolDown.fillAmount = 0.0f;
                    used = false;
                    limitText.text = limit.ToString();
                    if (limit == 0)
                    {
                        limitText.text = "";
                    }
                }
            }
        }
    }

    public void GodOfWashroom()
    {
        if (used == false && limit > 0)
        {
            if (timer.GetComponent<Timer>().timeLeft < 60)
            {
                sound[0].PlayOneShot(washroomAudioClips[6], 1.0f);
                //Debug.Log("Not enough time to go to washroom!");
            }
            else
            {
                limitText.text = "";
                limit--;
                timer.GetComponent<Timer>().timeLeft -= (60 - duration);
                used = true;

                setting.timeLeft = timer.GetComponent<Timer>().timeLeft;
                setting.setWashroom();
                setting.setQuestion();
                setting.setHint();

                projectile = setting.projectile;

                FadeOut();
                Invoke("Change", duration);
                Invoke("FadeIn", duration * 2);

                //LoadSceneHandler.SetActive(true);
            }
        }
        else
        {
            if (limit == 0)
            {
                sound[0].PlayOneShot(washroomAudioClips[5], 1.0f);
                //Debug.Log("You can just use this skill twice per test");
            }
            else
            {
                sound[0].PlayOneShot(washroomAudioClips[4], 1.0f);
                //Debug.Log("The skill is cooling down.");
            }
        }
    }

    public bool isTrigger()
    {
        return used;
    }

    public float GetCoolDownCounter()
    {
        return coolDownCounter;
    }

    public void ReduceCoolDownCounter(float n)
    {
        if (n == -1)
        {
            coolDownCounter = coolDown;
        }
        else
        {
            coolDownCounter -= n;
        }
    }

    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, duration);
    }

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, duration);
    }

    private void Change()
    {
        Washroom.SetActive(true);
        Level.SetActive(false);
        projectile.SetActive(false);
    }
}
