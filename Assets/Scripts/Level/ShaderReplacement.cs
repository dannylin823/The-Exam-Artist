using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderReplacement : MonoBehaviour
{
    public Shader shader;

    void Start()
    {
        gameObject.GetComponent<Camera>().SetReplacementShader(shader, "RenderType");
    }
}
