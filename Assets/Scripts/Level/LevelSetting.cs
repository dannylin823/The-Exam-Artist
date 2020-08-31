using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json.Linq;

public class LevelSetting : MonoBehaviour
{
    [Tooltip("This is used to store grabbed answer or not")]
    public bool answerGrabbed = false;
    [Tooltip("This is used to store talking and starting time")]
    public float offset, initialTime = 300.0f;
    [Tooltip("This is used to store remaining time")]
    public float timeLeft = -1.0f;
    [Tooltip("This is used to store answers")]
    public string[] answer;
    [Tooltip("This is used to determine if went to washroom")]
    public bool washroomed = false;
    [Tooltip("Set time staying in washroom")]
    public float washroomDuration = 10.0f;
    [Tooltip("This is used to store unanswered questions")]
    public List<int> unansweredQues;
    [Tooltip("This is used to store questions")]
    public GetQuestion question;
    [Tooltip("This is used to store current questions")]
    public MultipleChoiceBehavior[] quesTrack;
    [Tooltip("This is used to store score")]
    public int[] scoreTrack;
    [Tooltip("This is used to store hand and volume setting")]
    public Settings setting;
    [Tooltip("This is used to store subject")]
    public string subject;
    [Tooltip("This is used to store student positions")]
    public List<Vector3> positions = new List<Vector3>();
    [Tooltip("Set number of questions")]
    public int numQuestion = 5;
    [Tooltip("Check to enable random seats")]
    public bool randomseats = false;
    [Tooltip("This is used for randomseats")]
    public bool randomed = false;
    [Tooltip("This is used to store hints")]
    public List<JToken> hints = new List<JToken>();
    [Tooltip("Check to enable prepare page")]
    public bool onPrepare = true;
    [Tooltip("This is used to store bribe list")]
    public List<Sprite> bribeList = new List<Sprite>();
    [Tooltip("Check to enable illegal detection")]
    public bool illegalDetect = true;
    [Tooltip("Used to know if got caught")]
    public bool failed = false;
    [Tooltip("Used to know which projectile")]
    public GameObject projectile;

    private void Start()
    {
        timeLeft = -1.0f;
        question = null;
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
        projectile = GameObject.FindGameObjectWithTag("Projectile");
    }

    private void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
        }
        else
        {
            if (!answerGrabbed)
            {
                answer = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>().getAllAnswer();
                //setQuestion();
                answerGrabbed = true;
            }
        }
    }

    public void setTime()
    {
        timeLeft = GameObject.Find("Timer").GetComponent<Timer>().timeLeft;
    }

    public void setWashroom()
    {
        washroomed = true;
    }

    public void setQuestion()
    {
        TestPaperBehavior test = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        question = test.setQuestion();
        quesTrack = test.setQuesTrack();
        scoreTrack = test.setScoreTrack();
        unansweredQues = test.setUnansweredQues();
    }

    public void setHint()
    {
        MagicCheatSheetBehavior test = GameObject.Find("SkillsScript").GetComponent<MagicCheatSheetBehavior>();
        hints = test.setHints();
    }

    public void resetTemp()
    {
        //Debug.Log("reset");
        answerGrabbed = false;
        question = null;
        quesTrack = null;
        scoreTrack = null;
        unansweredQues = null;
        washroomed = false;
        timeLeft = -1.0f;
        offset = 15.0f;
        onPrepare = true;
        randomed = false;
        bribeList = new List<Sprite>();
    }

    public void setSubject(string s)
    {
        subject = s;
        GameObject.Find("Subject").GetComponent<Text>().text = s.ToUpper() + " TEST !";
        GameObject[] test = GameObject.FindGameObjectsWithTag("Subject");
        for(int i = 0; i< test.Length; i++)
        {
            test[i].GetComponent<Text>().text = s;
        }
    }

    public void setOnPrepare(bool b)
    {
        onPrepare = b;
    }

    public void setPositions(List<Vector3> positions)
    {
        this.positions = positions;
    }

    public void setFailed(bool b)
    {
        failed = b;
    }

}
