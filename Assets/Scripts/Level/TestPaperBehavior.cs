using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Valve.VR;

public class TestPaperBehavior : MonoBehaviour
{
    private GameObject preparePage, testPage, submitPage, initialPage, bribePage, bribeSkillPage;
    private GiftBlindEyesBehavior gbe;
    public GameObject questionTextObj, choiceA, choiceB, choiceC, choiceD;
    public GetQuestion question = new GetQuestion();
    public string JSON_file;
    public List<int> unansweredQues = new List<int>();
    private int tempQuestion = -1;
    private MultipleChoiceBehavior[] quesTrack;
    private int[] scoreTrack;
    private int bribePageCounter = -1;

    public int total_score = 0;
    public bool onPrepare = true;
    public Sprite[] studentImage;
    public GameObject LoadSceneHandler;

    private GameObject[] bribeOptions;
    private float offset;
    private bool start = false;
    private Color disabledColor = new Color(0.78f, 0.78f, 0.78f, 1.0f);
    private Color bribeColor = new Color(0.98f, 0.812f, 0.016f, 1.0f);
    private LevelSetting setting;

    void Start()
    {
        gbe = GameObject.Find("SkillsScript").GetComponent<GiftBlindEyesBehavior>();

        GameObject testPaper = GameObject.FindGameObjectWithTag("MainTestPaper");
        testPage = testPaper.transform.Find("TestPage").gameObject;
        submitPage = testPaper.transform.Find("SubmitPage").gameObject;
        initialPage = testPaper.transform.Find("InitialPage").gameObject;
        preparePage = testPaper.transform.Find("PreparePage").gameObject;
        bribePage = testPaper.transform.Find("BribePage").gameObject;
        bribeSkillPage = testPaper.transform.Find("BribeSkillPage").gameObject;

        preparePage.SetActive(true);
        initialPage.SetActive(false);
        testPage.SetActive(false);
        submitPage.SetActive(false);
        bribePage.SetActive(false);
        bribeSkillPage.SetActive(false);

        string[] files = { "World", "Chemistry", "Math", "Biology", "Computer", "History", "Music", "BioChemistry", "Sports", "Geography" };
        int index = Random.Range(0, files.Length);
        string filename = "questions-" + files[index] + ".json";
        JSON_file = filename;

        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        setting.setSubject(files[index]);

        onPrepare = setting.onPrepare;
        if (!onPrepare)
        {
            initialPage.SetActive(true);
            preparePage.SetActive(false);
        }

        offset = setting.offset;

        if (setting.washroomed)
        {
            question = setting.question;
            quesTrack = setting.quesTrack;
            scoreTrack = setting.scoreTrack;
            tempQuestion = question.current - 1;
            unansweredQues = setting.unansweredQues;
        }
        else
        {
            question.setNumQuestion(setting.numQuestion);
            question.readQuestionFromJson(JSON_file);
            //Debug.Log(question.ques);
            quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];
            scoreTrack = new int[question.getQuesCount()];
            for (int i = 0; i < question.getQuesCount(); i++)
            {
                scoreTrack[i] = 0;
                next();
            }
            for (int i = 0; i < setting.numQuestion; i++)
            {
                unansweredQues.Add(i);
            }
        }
    }

    void Update()
    {
        if (!onPrepare)
        {
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                if (!start)
                {
                    initialPage.SetActive(false);
                    testPage.SetActive(true);
                    next();
                    start = true;
                }
            }
        } 
    }

    public int getCurrentQuesNum()
    {
        return tempQuestion;
    }

    public int getCurrentQuesId()
    {
        return int.Parse(question.getQuestionId(tempQuestion));
    }

    public int getCurrentQuesAns()
    {
        return quesTrack[tempQuestion].correctAns;
    }

    public void next()
    {
        reset();

        if (tempQuestion < question.getQuesCount() - 1)
        {
            tempQuestion += 1;
        }
        else
        {
            tempQuestion = 0;
        }
        //Debug.Log(tempQuestion);
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getNextQuestion();
            //Debug.Log(Q);
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        question.updateQuesNum(tempQuestion);
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
        if (quesTrack[tempQuestion].choice > -1)
        {
            setSelectedTag(quesTrack[tempQuestion].choice);
        }
    }

    public void previous()
    {
        if (tempQuestion != -1)
        {
            if (quesTrack[tempQuestion].isCorrect != scoreTrack[tempQuestion])
            {
                if (scoreTrack[tempQuestion] == 0) total_score += 1;
                else total_score -= 1;
                scoreTrack[tempQuestion] = quesTrack[tempQuestion].isCorrect;
            }
        }

        reset();

        if (tempQuestion > 0)
        {
            tempQuestion -= 1;
        }
        else
        {
            tempQuestion = question.getQuesCount() - 1;
        }
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getPrevQuestion();
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        question.updateQuesNum(tempQuestion);
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
        if(quesTrack[tempQuestion].choice > -1)
        {
            setSelectedTag(quesTrack[tempQuestion].choice);
        }
    }

    public void startTest()
    {
        preparePage.SetActive(false);
        initialPage.SetActive(true);
        onPrepare = false;
        setting.setOnPrepare(false);
        Text subjectText = GameObject.FindGameObjectWithTag("MainSubject").GetComponent<Text>();
        subjectText.text = setting.subject;
       
    }

    public void openBribePage()
    {
        preparePage.SetActive(false);
        bribePage.SetActive(true);
        if (bribeOptions == null || bribeOptions.Length == 0)
        {
            bribeOptions = GameObject.FindGameObjectsWithTag("BribeOption");
        }

        bribePageNext();
    }

    public void bribePageNext()
    {
        //Debug.Log(Mathf.Floor(studentImage.Length / bribeOptions.Length));
        //Debug.Log((float)studentImage.Length / bribeOptions.Length);
        if (bribePageCounter + 1 < Mathf.Ceil((float)studentImage.Length / bribeOptions.Length))
        {
            bribePageCounter += 1;
        }
        else
        {
            bribePageCounter = 0;
        }
        showStudentImage();
    }

    public void bribePagePrev()
    {
        if (bribePageCounter > 0)
        {
            bribePageCounter -= 1;
        }
        else
        {
            bribePageCounter = (int)Mathf.Ceil((float)studentImage.Length / bribeOptions.Length) - 1;
        }
        showStudentImage();
    }

    private void showStudentImage()
    {
        for (int i = 0; i < bribeOptions.Length; i++)
        {
            if (bribePageCounter * bribeOptions.Length + i < studentImage.Length)
            {
                Image img = bribeOptions[i].GetComponent<Image>();
                img.sprite = studentImage[bribePageCounter * bribeOptions.Length + i];
                bribeOptions[i].transform.parent.parent.gameObject.SetActive(true);
                if (gbe.bribeList.Contains(img.sprite))
                {
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Text>().text = "Cancel";
                }
                else
                {
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Text>().text = "Bribe";
                }
                if (gbe.bribeList.Count == 3)
                {
                    if (bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Text>().text == "Bribe")
                    {
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Button>().enabled = false;
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Image>().color = disabledColor;
                    }
                    else
                    {
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Button>().enabled = true;
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Image>().color = bribeColor;
                    }
                }
                else
                {
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Button>().enabled = true;
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Image>().color = bribeColor;
                }
            }
            else
            {
                bribeOptions[i].transform.parent.parent.gameObject.SetActive(false);
            }

        }
    }

    public void showBribeSkillPage()
    {
        if (!gbe.isCoolDown())
        {
            testPage.SetActive(false);
            bribeSkillPage.SetActive(true);
            for (int i = 0; i < gbe.bribeList.Count; i++)
            {
                bribeSkillPage.transform.GetChild(i).gameObject.SetActive(true);
                bribeSkillPage.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite = gbe.bribeList[i];
            }
        }
        else
        {
            Debug.Log("The bribing system is cooling down!");
        }
        
    }

    public void bribeStudent(GameObject target)
    {
        if (target.transform.GetChild(1).GetComponentInChildren<Text>().text == "Bribe")
        {
            target.transform.GetChild(1).GetComponentInChildren<Text>().text = "Cancel";
            gbe.bribeList.Add(target.transform.GetChild(0).GetComponentInChildren<Image>().sprite);
        }
        else
        {
            target.transform.GetChild(1).GetComponentInChildren<Text>().text = "Bribe";
            gbe.bribeList.Remove(target.transform.GetChild(0).GetComponentInChildren<Image>().sprite);
        }
        GameObject options = target.transform.parent.gameObject;
        for (int i = 0; i < bribeOptions.Length; i++)
        {
            GameObject temp = options.transform.GetChild(i).gameObject;
            if (temp.transform.GetChild(1).GetComponentInChildren<Text>().text == "Bribe")
            {
                if (gbe.bribeList.Count == 3)
                {
                    temp.transform.GetChild(1).GetComponentInChildren<Button>().enabled = false;
                    temp.transform.GetChild(1).GetComponentInChildren<Image>().color = disabledColor;
                }
                else
                {
                    temp.transform.GetChild(1).GetComponentInChildren<Button>().enabled = true;
                    temp.transform.GetChild(1).GetComponentInChildren<Image>().color = bribeColor;
                }
            }
        }
    }

    public void ChooseBribee(GameObject t)
    {
        gbe.ChooseBribee(t);
        backToTest();
    }

    public void backToPreparePage()
    {
        bribePage.SetActive(false);
        preparePage.SetActive(true);
        bribePageCounter = -1;
        //Debug.Log(gbe.bribeList.Count);
    }

    public bool isBribeSkillActive()
    {
        return bribeSkillPage.activeSelf;
    }

    public bool isBribeActive()
    {
        return bribePage.activeSelf;
    }

    public void submit()
    {
        Button submit = testPage.GetComponentsInChildren<Button>()[6];
        ColorBlock cb = submit.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        submit.colors = cb;

        testPage.SetActive(false);
        submitPage.SetActive(true);
    }

    public void backToTest()
    {
        if (submitPage.activeSelf)
        {
            Button no = submitPage.GetComponentsInChildren<Button>()[1];
            ColorBlock cb = no.colors;
            cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            no.colors = cb;
            submitPage.SetActive(false);
        }
        if (bribeSkillPage.activeSelf)
        {
            bribeSkillPage.SetActive(false);
        }

        testPage.SetActive(true);
    }

    public GetQuestion setQuestion()
    {
        return question;
    }
    public MultipleChoiceBehavior[] setQuesTrack()
    {
        return quesTrack;
    }
    public int[] setScoreTrack()
    {
        return scoreTrack;
    }

    public List<int> setUnansweredQues()
    {
        return unansweredQues;
    }

    public string[] getAllAnswer()
    {
        int size = question.getQuesCount();
        string[] answer = new string[size];
        char[] abcd = { 'A', 'B', 'C', 'D' };
        for (int i = 0; i < size; i++)
        {
            char ans = abcd[question.getQuestionCorrectAns(i)];
            answer[i] = (i + 1).ToString() + ": " + ans;
        }
        return answer;
    }

    public void gameOver()
    {
        testPage.SetActive(true);
        testPage.transform.Find("question").gameObject.SetActive(false);
        testPage.transform.Find("choices").gameObject.SetActive(false);
        testPage.transform.Find("PageNavigator").gameObject.SetActive(false);
        testPage.transform.Find("SubmitTool").gameObject.SetActive(false);
        testPage.transform.Find("GameOverText").gameObject.SetActive(true);
        submitPage.SetActive(false);
        bribeSkillPage.SetActive(false);
    }


    public void writeAnsToJson()
    {
        char[] abcd = { 'A', 'B', 'C', 'D' };
        using (StreamWriter file = File.CreateText(@Application.dataPath + "/GameData/answers.json"))

        using (JsonWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;

            writer.WriteStartObject();
            writer.WritePropertyName("Answers");
            writer.WriteStartArray();
            for (int i = 0; i < question.getQuesCount(); i++)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("id");
                writer.WriteValue(question.getQuestionId(i));
                // writing question_txt to the answers.json file
                writer.WritePropertyName("question_txt");
                writer.WriteValue(question.getQuestionTxt(i));
                writer.WritePropertyName("YourAns");
                //Debug.Log(i);
                //Debug.Log(quesTrack.Length);
                if (quesTrack[i] == null || quesTrack[i].choice == -1) writer.WriteValue("NA");
                else writer.WriteValue(abcd[quesTrack[i].choice] + ". " + quesTrack[i].choiceContent);
                writer.WritePropertyName("MyAns");
                //Debug.Log(question.getQuestionCorrectAns(i));
                writer.WriteValue(abcd[question.getQuestionCorrectAns(i)] + ". " + question.getQuestionCorrectAnsContext(i));
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        if (SteamVR.active)
        {
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            Initiate.Fade("GameOver", Color.black, 0.5f);
        }
    }

    public void setSelectedTag(int i)
    {
        Button btn = choiceA.GetComponentInChildren<Button>();
        switch (i)
        {
            case 1:
                btn = choiceB.GetComponentInChildren<Button>();
                break;
            case 2:
                btn = choiceC.GetComponentInChildren<Button>();
                break;
            case 3:
                btn = choiceD.GetComponentInChildren<Button>();
                break;

        }
        btn.tag = "MainChoiceSelected";
    }

    public void reset()
    {
        Button A = choiceA.GetComponentInChildren<Button>();
        Button B = choiceB.GetComponentInChildren<Button>();
        Button C = choiceC.GetComponentInChildren<Button>();
        Button D = choiceD.GetComponentInChildren<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        A.colors = cb;
        B.colors = cb;
        C.colors = cb;
        D.colors = cb;
        A.tag = "MainChoice";
        B.tag = "MainChoice";
        C.tag = "MainChoice";
        D.tag = "MainChoice";
    }

     public void selectA()
    {
        reset();
        choiceA.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceA, 0);
        unansweredQues.Remove(tempQuestion);
    }
    public void selectB()
    {
        reset();
        choiceB.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceB, 1);
        unansweredQues.Remove(tempQuestion);
    }
    public void selectC()
    {
        reset();
        choiceC.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceC, 2);
        unansweredQues.Remove(tempQuestion);
    }
    public void selectD()
    {
        reset();
        choiceD.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceD, 3);
        unansweredQues.Remove(tempQuestion);
    }

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, 0.0f);
        SteamVR_Fade.Start(Color.black, 2.0f);
    }
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, 0.0f);
        SteamVR_Fade.Start(Color.clear, 2.0f);
        SceneManager.LoadScene(2);
    }

    public bool isStart()
    {
        return start;
    }
}
