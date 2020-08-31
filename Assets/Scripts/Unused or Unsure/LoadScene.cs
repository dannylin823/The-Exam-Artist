using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void SceneLoader(int sceneIndex)
    {
        Destroy(GameObject.FindGameObjectWithTag("MainPlayer"));
        SceneManager.LoadScene(sceneIndex);
    }
}
