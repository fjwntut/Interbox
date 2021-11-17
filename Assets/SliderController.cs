using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    Slider slider;
    public Text value;
    public 
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate
        {
            SliderValueChanged(slider);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SliderValueChanged(Slider slider)
    {
        if(slider.value == 360)
        {
            slider.value = 0;
        }
    }
}
