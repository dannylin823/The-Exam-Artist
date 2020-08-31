using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeat : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    private List<string> names = new List<string>();
    static System.Random random = new System.Random();

    void Start()
    {
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        if (setting.randomseats)
        {
            getPosition();
            if (!setting.randomed)
            {
                for (int i = 0; i < 12; i++)
                {
                    positions.Add(GameObject.Find(names[i]).transform.position);
                }
                randomize();
                for (int i = 0; i < 12; i++)
                {
                    GameObject.Find(names[i]).transform.position = positions[i];
                }
                setting.randomed = true;
                setting.setPositions(positions);
            }
            else
            {
                positions = setting.positions;
                for (int i = 0; i < 12; i++)
                {
                    GameObject.Find(names[i]).transform.position = positions[i];
                }
            }
        }
    }

    void randomize()
    {
        int n = positions.Count;
        for (int i = 0; i < (n - 1); i++)
        {
            int r = i + random.Next(n - i);
            Vector3 t = positions[r];
            positions[r] = positions[i];
            positions[i] = t;
        }
    }

    void getPosition()
    {
        GameObject[] students = GameObject.FindGameObjectsWithTag("Student");
        GameObject player = GameObject.FindGameObjectWithTag("MainPlayerPosition");
        
        for (int i = 0; i < students.Length; i++)
        {
            names.Add(students[i].name);
        }
        names.Add(player.name);
    }
}
