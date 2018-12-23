using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
    Text fpsText;

    void Start()
    {
        fpsText = GetComponent<Text>();
    }
    
    void Update()
    {
        fpsText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
    }
}
