using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ArduinoRead : MonoBehaviour
{
    public bool calibrate;
    public Vector3Int boxMultiplier = new Vector3Int(1, 0, 0);
    Quaternion boxInitPos;
    public Transform door;
    public Vector3Int doorMultiplier = new Vector3Int(0, 0, 1);
    Quaternion DoorInitPos;
    private void Start()
    {
        boxInitPos = transform.rotation;
        DoorInitPos = door.localRotation;
        UduinoManager.Instance.OnDataReceived += DataReceived;
    }

    private void DataReceived(string reading, UduinoDevice board)
    {
        string[] data = reading.Split('\\');
        float boxAngle = float.Parse(data[0]);
        float doorAngle = float.Parse(data[1]);
        door.localRotation = DoorInitPos * Quaternion.Euler(new Vector3(doorAngle * doorMultiplier.x, doorAngle * doorMultiplier.y, doorAngle * doorMultiplier.z));
        transform.rotation = boxInitPos * Quaternion.Euler(new Vector3(boxAngle * boxMultiplier.x, boxAngle * boxMultiplier.y, boxAngle * boxMultiplier.z));
        Debug.Log($"box rotation: {transform.eulerAngles}; door rotation: {door.transform.eulerAngles}");
    }
    private void Update()
    {
        if (calibrate)
        {
            calibrate = false;
            UduinoManager.Instance.sendCommand("0");
        }
    }
}
