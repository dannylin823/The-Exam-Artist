using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Valve.VR;

public class MeditationHandler : MonoBehaviour
{
    public GameObject level, projectile;
    public GameObject front, right, left, back;
    public MeditationBehavior meditation;

    private float time = 20.0f;
    private Text[] frontText, rightText, leftText, backText, texts;
    private List<bool> show = new List<bool>();
    private Dictionary<int, Text> showing = new Dictionary<int, Text>(), fading = new Dictionary<int, Text>();
    private List<int> remove = new List<int>();
    private AudioSource[] sound;
    private bool play = false, extended = false;
    private JObject words_odj;
    private JArray words;
    private float duration = 2.0f;
    private LevelSetting setting;
    private bool isMeditate = false;


    void Awake()
    {
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        frontText = front.GetComponentsInChildren<Text>();
        rightText = right.GetComponentsInChildren<Text>();
        leftText = left.GetComponentsInChildren<Text>();
        backText = back.GetComponentsInChildren<Text>();

        texts = new Text[frontText.Length + rightText.Length + leftText.Length + backText.Length];
        frontText.CopyTo(texts, 0);
        rightText.CopyTo(texts, frontText.Length);
        leftText.CopyTo(texts, frontText.Length + rightText.Length);
        backText.CopyTo(texts, frontText.Length + rightText.Length + leftText.Length);

        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/Meditation.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            words_odj = (JObject)JToken.ReadFrom(reader);
        }
        words = (JArray)words_odj["words"];
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
    }

    private void OnEnable()
    {
        isMeditate = true;
        if (!extended)
        {
            for (int i = 0; i < setting.question.getQuesCount(); i++)
            {
                JToken t = setting.question.getQuestionTxt(i);
                words.Add(t);
            }
        }
        sound[2].volume = 0.5f;
        for (int i = 0; i < texts.Length; i++)
        {
            Color c = texts[i].color;
            c.a = 0.0f;
            texts[i].color = c;
            int index = Random.Range(0, words.Count);
            texts[i].text = (string)words[index];
            show.Add(false);
        }
        projectile = setting.projectile;
    }

    void Update()
    {
        if (time > 0)
        {
            if (time < 5.0f)
            {
                if (!play)
                {
                    StartCoroutine(FadeOut(sound[2], 5.0f));
                    sound[0].PlayOneShot(meditation.meditationAudioClips[meditation.correctAns], 1.0f);
                    play = true;
                }
            }

            time -= Time.deltaTime;
            remove = new List<int>();
            foreach (KeyValuePair<int, Text> p in showing)
            {
                FadeTextToFullAlpha(1.0f, p.Value, p.Key);
            }
            for (int i = 0; i < remove.Count; i++)
            {
                showing.Remove(remove[i]);
            }

            remove = new List<int>();
            foreach (KeyValuePair<int, Text> p in fading)
            {
                FadeTextToZeroAlpha(1.0f, p.Value, p.Key);
            }
            for (int i = 0; i < remove.Count; i++)
            {
                fading.Remove(remove[i]);
            }

            int index = Random.Range(0, texts.Length);
            if (!showing.ContainsKey(index) && !fading.ContainsKey(index))
            {
                if (!show[index])
                {
                    /*string[] ans = { "A", "B", "C", "D" };
                    if (time < 5.0f)
                    {
                        texts[index].text = ans[meditation.correctAns];
                    }*/
                    FadeTextToFullAlpha(1.0f, texts[index], index);
                }
                else
                {
                    FadeTextToZeroAlpha(0.1f, texts[index], index);
                }
            }
        }
        else
        {
            sound[2].Stop();
            time = 15.0f;
            play = false;
            FadeOut();
            Invoke("FadeIn", duration);
        }
    }

    public void FadeTextToFullAlpha(float t, Text i, int index)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
        if (!showing.ContainsKey(index))
        {
            showing.Add(index, i);
        }
        if (i.color.a >= 1.0f)
        {
            show[index] = true;
            remove.Add(index);
        }
    }

    public void FadeTextToZeroAlpha(float t, Text i, int index)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
        if (!fading.ContainsKey(index))
        {
            fading.Add(index, i);
        }
        if (i.color.a <= 0.0f)
        {
            show[index] = false;
            remove.Add(index);
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }



    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, duration);
    }

    private void FadeIn()
    {
        gameObject.SetActive(false);
        level.SetActive(true);
        isMeditate = false;
        projectile.SetActive(true);
        SteamVR_Fade.Start(Color.clear, duration);
    }

    public bool inMeditation()
    {
        return isMeditate;
    }

}
