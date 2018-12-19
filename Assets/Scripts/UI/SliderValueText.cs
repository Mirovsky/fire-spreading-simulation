using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    [SerializeField]
    Text sliderValue;
    [SerializeField]
    Slider slider;

    void Start()
    {
        slider.onValueChanged.AddListener(SliderChanged);    
    }

    void SliderChanged(float value)
    {
        sliderValue.text = value.ToString();
    }
}
