using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindRoseController : MonoBehaviour
{
    [SerializeField]
    Image arrow;

    RectTransform arrowTransform;
    
    public void WindSpeedChanged(float speed)
    {
        var scale = arrowTransform.localScale;

        scale.x = speed / 100;

        arrowTransform.localScale = scale;
    }

    public void WindDirectionChange(float direction)
    {
        var rotation = arrowTransform.localRotation;
        var euler = rotation.eulerAngles;

        euler.z = direction;

        rotation.eulerAngles = euler;
        arrowTransform.localRotation = rotation;
    }

    void Start()
    {
        arrowTransform = arrow.GetComponent<RectTransform>();

        WindSpeedChanged(100f);
        WindDirectionChange(0);
    }

}
