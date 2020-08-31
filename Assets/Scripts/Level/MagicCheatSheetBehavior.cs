using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class MagicCheatSheetBehavior : MonoBehaviour
{
    private JArray hintArray;
    public GameObject testPaper;
    public Image imgCoolDown, imgExist;
    public Text textCoolDown;
    
    private float coolDown = 5.0f, coolDownCounter = 5.0f;
    private float existTime = 5.0f, existTimeCounter = 5.0f;
    private bool exist = false, used = false; //increased = false;
    private Text currentHintShown;
    private Text cheatText;
    static string[] choices = { "A", "B", "C", "D" };
    //private Text tableHint;
    private HintsHandle hintHandle;
    private List<JToken> hintJToken = new List<JToken>();
    private TimeFreezeBehavior tf;

    private void loadResources()
    {
        GameObject table = GameObject.Find("PlayerTable");
        GameObject SkillsOverlay = table.transform.Find("SkillsOverlay").gameObject;
        GameObject SkillCoolDown = SkillsOverlay.transform.Find("SkillCoolDown").gameObject;
        GameObject skill = SkillCoolDown.transform.Find("MagicCheatSheet").gameObject;
        GameObject resources = skill.transform.Find("Image").gameObject;

        imgCoolDown = resources.transform.Find("CDImg").gameObject.GetComponent<Image>();
        imgExist = resources.transform.Find("exsiting").gameObject.GetComponent<Image>();
        textCoolDown = resources.transform.Find("CDText").gameObject.GetComponent<Text>();

        GameObject cheatsheet = GameObject.Find("CheatSheet");
        cheatText = cheatsheet.transform.Find("Hint").GetComponentInChildren<Text>();

        //tableHint = GameObject.Find("TableHint").GetComponentInChildren<Text>();
        hintHandle = GameObject.Find("HintHandle").GetComponent<HintsHandle>();

        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    void Start()
    {
        loadResources();

        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        //tableHint.text = "";
        cheatText.text = "";

        GameObject[] hints = GameObject.FindGameObjectsWithTag("Hint");
        for(int i = 0; i < hints.Length; i++)
        {
            hints[i].GetComponentInChildren<Text>().text = "";
        }

        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/hints.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            hintArray = (JArray)((JObject)JToken.ReadFrom(reader))["questions"];
        }
        RandomHint();
    }

    void Update()
    {
        if (!tf.hold)
        {
            if (existTimeCounter > 0 && exist == true && used == false)
            {
                existTimeCounter -= Time.deltaTime;
                imgExist.fillAmount = 1 - existTimeCounter / existTime;
                textCoolDown.text = ((int)Mathf.Ceil(existTimeCounter)).ToString();
            }
            else if (existTimeCounter <= 0 && exist == true && used == false)
            {
                existTimeCounter = existTime;
                exist = false;
                imgExist.fillAmount = 0.0f;
                //tableHint.text = "";
                cheatText.text = "";
                currentHintShown.text = "";
                used = true;
            }
            else if (coolDownCounter > 0 && used == true)
            {
                coolDownCounter -= Time.deltaTime;
                imgCoolDown.fillAmount = 1 - coolDownCounter / coolDown;
                textCoolDown.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
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

    public void MagicCheatSheet()
    {
        if (exist == false && used == false)
        {
            int temp_ques_id = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesId();
            //Debug.Log(temp_ques_id);
            //Debug.Log((JObject)hintArray[temp_ques_id]);
            //string hintStr = (string)((JObject)hintArray[temp_ques_id])["hints"];
            int index = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesNum();
            string hintStr = (string) hintJToken[index]["hints"];
            //tableHint.text = (testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesNum() + 1).ToString() + ". " + hintStr;
            cheatText.text = hintStr;

            exist = true;
            int tempAnsIdx = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesAns();
            //int id = (int)((JObject)hintArray[temp_ques_id])["id"];
            int id = (int) hintJToken[index]["id"];

            hintHandle.Hints[id].text = choices[tempAnsIdx];
            currentHintShown = hintHandle.Hints[id];
            if (id == 8)
            {
                GameObject.Find("OutsideAnswer").GetComponentInChildren<femaleoutside>().startAnimation();
                GameObject.Find("OutsideAnswer").GetComponentInChildren<femaleoutside>().enabled = true;
            }
        }
        else
        {
            Debug.Log("Your skill need to be cooled down");
        }
    }

    public bool isTrigger()
    {
        return used == true || exist == true;
    }

    public void ResetSkill()
    {
        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        //tableHint.text = "";
        cheatText.text = "";
        if (currentHintShown)
        {
            currentHintShown.text = "";
        }

        coolDownCounter = coolDown;
        existTimeCounter = existTime;
        exist = false;
        used = false;
    }

    public void RandomHint()
    {
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        if (setting.hints.Count != 0)
        {
            hintJToken = setting.hints;
        }
        else{
            int numQuestion = setting.numQuestion;
            for (int i = 0; i < numQuestion; i++)
            {
                int id = Random.Range(0, hintArray.Count);
                hintJToken.Add(hintArray[id]);
            }
        }
    }

    public List<JToken> setHints()
    {
        return hintJToken;
    }
}
