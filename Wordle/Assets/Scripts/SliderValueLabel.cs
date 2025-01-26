using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueLabel : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Slider slider;

    void Update()
    {
        text.text = slider.value+"";
    }
}
