using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{
    [Range(0f, 180f)]
    public float boxRotation;
    [Range(0f, 90f)]
    public float doorRotation;
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
        transform.localEulerAngles = originalBox + boxMultiplier * boxRotation;
        door.localEulerAngles = originalDoor + doorRotation * doorMultiplier;
        if ((doorRotation == 0) != doorClosed)
        {
            doorClosed = (doorRotation == 0);
        }
    }
}
