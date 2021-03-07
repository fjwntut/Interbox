using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ArduinoRead : MonoBehaviour
{
    public bool calibrate;
    public Vector3Int boxMultiplier = new Vector3Int(1, 0, 0);
    Quaternion boxInitPos;
    float boxAngle;
    public Transform door;
    public Vector3Int doorMultiplier = new Vector3Int(0, 0, 1);
    Quaternion DoorInitPos;
    float doorAngle;
    Animator animator;
    bool doorClosed
    {
        get
        {
            return animator.GetBool("doorClosed");
        }
        set
        {
            animator.SetBool("doorClosed", value);
        }
    }
    private void Start()
    {
        boxInitPos = transform.rotation;
        DoorInitPos = door.localRotation;
        UduinoManager.Instance.OnDataReceived += DataReceived;
        animator = gameObject.GetComponent<Animator>();
        doorAngle = 0;
        doorClosed = true;
    }

    private void DataReceived(string reading, UduinoDevice board)
    {
        string[] data = reading.Split('\\');
        boxAngle = float.Parse(data[0]);
        doorAngle = float.Parse(data[1]);
        door.localRotation = DoorInitPos * Quaternion.Euler(new Vector3(doorAngle * doorMultiplier.x, doorAngle * doorMultiplier.y, doorAngle * doorMultiplier.z));
        transform.rotation = boxInitPos * Quaternion.Euler(new Vector3(boxAngle * boxMultiplier.x, boxAngle * boxMultiplier.y, boxAngle * boxMultiplier.z));
        Debug.Log($"box rotation: {boxAngle}; door rotation: {doorAngle}");
    }
    private void Update()
    {
        if (doorClosed != (doorAngle > -0.5f))
        {
            doorClosed = !doorClosed;
        }
    }
}
