using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


[System.Serializable]
public class GetQuestion
{
    public JArray ques;
    public int current = -1;

    private string jsonString;
    private JObject ques_obj;
    private int numQuestion = 5, numFileQuestion;

    static System.Random _random = new System.Random();

    public GetQuestion copy()
    {
        GetQuestion res = new GetQuestion();
        res.jsonString = jsonString;
        res.ques_obj = ques_obj;
        res.ques = ques;
        return res;
    }
    
    public void readQuestionFromJson(string data)
    {
        // read JSON directly from a file
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/" + data))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            ques_obj = (JObject)JToken.ReadFrom(reader);
        }

        // Now all our data is on ques_obj
        // IMPORTANT : THIS INE WILL PRINT YOUR RESULT
        getQuestionsArray();
        //Debug.Log(getNextQuestion());
        //Debug.Log(getNextQuestion());
    }

    public JObject getNextQuestion()
    {
        current = (current < ques.Count - 1) ? (++current) : 0;
        /*if (current < ques.Count - 1)
        {
            current += 1;
        }
        else
        {
            current = 0;
        }*/
        return (JObject)ques[current];
    }

    public JObject getPrevQuestion()
    {
        if (current > 0)
        {
            current -= 1;
        }
        else
        {
            current = ques.Count - 1;
        }
        //Debug.Log(current);
        return (JObject)ques[current];
    }

    public void updateQuesNum(int n)
    {
        current = n;
    }

    public string getQuestionId(int idx)
    {
        return (string)ques[idx]["id"];
    }

    //to get question text using the idx
    public string getQuestionTxt(int idx)
    {
        return (string)ques[idx]["question_txt"];
    }

    public int getQuestionCorrectAns(int idx)
    {
        JArray options = (JArray)ques[idx]["options"];
        for (int i = 0; i < options.Count; i++)
        {
            if ((string)((JObject)options[i])["isCorrect"] == "True")
            {
                return i;
            }
        }
        //Debug.Log("Error");
        return -1;
    }

    public string getQuestionCorrectAnsContext(int idx)
    {
        JArray options = (JArray)ques[idx]["options"];
        for (int i = 0; i < options.Count; i++)
        {
            if ((string)((JObject)options[i])["isCorrect"] == "True")
            {
                return (string)((JObject)options[i])["option_txt"];
            }
        }
        //Debug.Log("Error");
        return "";
    }

    //Call this function to get your questions array in JArray format
    public void getQuestionsArray()
    {
        ques = (JArray)ques_obj["questions"];
        numFileQuestion = ques.Count;
        Shuffle(ques);
        for (int i = 0; i < ques.Count; i++)
        {
            JArray t = (JArray)ques[i]["options"];
            Shuffle(t);
            ques[i]["options"] = t;
        }
        JArray temp = new JArray();
        if(numQuestion > ques.Count)
        {
            numQuestion = ques.Count;
        }
        else if(numQuestion < 0)
        {
            numQuestion = 1;
        }

        for (int i = 0; i < numQuestion; i++)
        {
            temp.Add(ques[i]);
        }
        ques = temp;
    }

    public int getQuesCount()
    {
        return ques.Count;
    }

    // This function shuffes the question order everytime
    static void Shuffle(JArray array)
    {
        int n = array.Count;
        for (int i = 0; i < (n - 1); i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + _random.Next(n - i);
            JToken t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    public void setNumQuestion(int numQuestion)
    {
        this.numQuestion = numQuestion;
    }

    public int getNumFileQuestion()
    {
        return numFileQuestion;
    }


}
