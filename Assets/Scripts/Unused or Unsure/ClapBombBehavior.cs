using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapBombBehavior : MonoBehaviour
{
    private System.Random r = new System.Random();
    public GameObject targetStudentPos = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClapBomb()
    {
        int rInt = r.Next(0, 4);
        GameObject[] bombPostions = GameObject.FindGameObjectsWithTag("ProjectileTestPosition");
        GameObject p = bombPostions[rInt];
        Vector2 p_xz = new Vector2(p.transform.position.x, p.transform.position.z);

        float minDist = 100000000.0f;
        int minStuIdx = -1;
        GameObject[] students = GameObject.FindGameObjectsWithTag("StudentCharacter");
        GameObject[] studentPositions = GameObject.FindGameObjectsWithTag("StudentPosition");
        for (int i = 0; i < students.Length; i++)
        {
            Vector2 s_xz = new Vector2(students[i].transform.position.x, students[i].transform.position.z);
            if (Vector2.Distance(s_xz, p_xz) < minDist)
            {
                minDist = Vector2.Distance(s_xz, p_xz);
                minStuIdx = i;
            }
        }
        targetStudentPos = studentPositions[minStuIdx];
        Debug.Log(studentPositions[minStuIdx].name);
    }
}
