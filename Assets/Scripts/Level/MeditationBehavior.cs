using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class MeditationBehavior : MonoBehaviour
{
    public Image imgCoolDown;
    public Text textCoolDown;
    public GameObject timer;
    public GameObject testPaper;
    private AudioSource[] sound;
    public AudioClip[] meditationAudioClips = new AudioClip[4];
    private float coolDown = 15.0f;
    private float coolDownCounter = 15.0f;
    private bool used = false, enoughTime = true;
    //private int limit = 5;
    private float duration = 2.0f;
    public GameObject wall, level, player, projectile;
    public int correctAns;
    private LevelSetting setting;
    private TimeFreezeBehavior tf;

    void Start()
    {
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        level = GameObject.Find("Level");
        player = GameObject.FindGameObjectWithTag("MainPlayer");
        imgCoolDown.fillAmount = 0.0f;
        textCoolDown.text = "";
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();

        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    void Update()
    {
        if (!tf.hold)
        {
            if (timer.GetComponent<Timer>().timeLeft < 60 && enoughTime)
            {
                imgCoolDown.fillAmount = 1.0f;
                imgCoolDown.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
                textCoolDown.text = "";
                enoughTime = false;
            }
            else if (enoughTime)
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
                }
            }
        }
    }

    public void Meditation()
    {
        if (!used && enoughTime)
        {
            timer.GetComponent<Timer>().timeLeft -= (60 - duration);
            used = true;

            GameObject skills = GameObject.Find("SkillsScript");
            GodOfWashroomBehavior gow = skills.GetComponent<GodOfWashroomBehavior>();
            MagicCheatSheetBehavior mcs = skills.GetComponent<MagicCheatSheetBehavior>();
            GiftBlindEyesBehavior gbe = skills.GetComponent<GiftBlindEyesBehavior>();

            setting.setQuestion();

            if (gow.isTrigger() == true)
            {
                gow.ReduceCoolDownCounter(60);
            }

            if (mcs.isTrigger())
            {
                mcs.ResetSkill();
            }

            if (gbe.isCoolDown())
            {
                gbe.ReduceCoolDownCounter(60);
            }

            projectile = setting.projectile;

            FadeOut();
            Invoke("FadeIn", duration);

            correctAns = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesAns();
            //sound[0].PlayOneShot(meditationAudioClips[correctAns], 1.0f);
        }
        /*else
        {
            Debug.Log("The skill is cooling down.");
        }*/
    }

    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, duration);
    }

    private void FadeIn()
    {
        level.SetActive(false);
        wall.SetActive(true);
        projectile.SetActive(false);
        SteamVR_Fade.Start(Color.clear, duration);
    }

    public bool isTrigger()
    {
        return used;
    }
}
