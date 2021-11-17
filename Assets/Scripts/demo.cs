using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class demo : MonoBehaviour
{
    public Slider boxSlider;
    public Text boxValue;
    public Slider doorSlider;
    public Text doorValue;
    public Toggle useDemoToggle;
    /*
    [Range(0f, 360f)]
    public float boxRotation;
    [Range(0f, 90f)]
    public float doorRotation;
    */
    public Transform door;
    Animator animator;
    Vector3 originalDoor, originalBox;
    public Vector3 doorMultiplier, boxMultiplier;
    bool doorClosed
    {
        get
        {
            return (animator.GetBool("doorClosed"));
        }
        set
        {
            animator.SetBool("doorClosed", value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        originalBox = transform.localEulerAngles;
        originalDoor = door.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        bool useDemo = useDemoToggle.isOn;
        float boxRotation = boxSlider.value;
        boxValue.text = "" + boxRotation;
        float doorRotation = doorSlider.value;
        doorValue.text = "" + doorRotation;
        if(GetComponent<ArduinoRead>().enabled == useDemo)
        {
            GetComponent<ArduinoRead>().enabled = !useDemo;
        }
        if (useDemo)
        {
            transform.localEulerAngles = originalBox + boxMultiplier * boxRotation;
            door.localEulerAngles = originalDoor + doorRotation * doorMultiplier;
            if ((doorRotation == 0) != doorClosed)
            {
                doorClosed = (doorRotation == 0);
            }
        }
        
        
    }
}
