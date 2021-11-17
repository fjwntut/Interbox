using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateAllDisplays : MonoBehaviour
{
    public Text display;
    void Start()
    {
        display.text = "displays connected: " + Display.displays.Length;
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    void Update()
    {

    }
}