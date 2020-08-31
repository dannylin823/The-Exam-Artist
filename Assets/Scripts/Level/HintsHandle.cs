using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class HintsHandle : MonoBehaviour
{
    public int numHint;
    public List<Text> Hints = new List<Text>();
    private List<string> objName = new List<string>();

    private void Awake()
    {
        JArray hintArray;
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/hints.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            hintArray = (JArray)((JObject)JToken.ReadFrom(reader))["questions"];
        }
        numHint = hintArray.Count;

        for(int i = 0; i < numHint; i++)
        {
            string name = (string)hintArray[i]["name"];
            Hints.Add(GameObject.Find(name).GetComponentInChildren<Text>());
        }
    }
}
