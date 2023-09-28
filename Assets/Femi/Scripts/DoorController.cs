using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator doorAnimator;
    bool triggerEntered;
    void Start()
    {
        doorAnimator = GameObject.Find("Door Body").GetComponent<Animator>();
        triggerEntered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEntered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        triggerEntered = false;
        CloseDoor();
    }

    public void OpenDoor()
    {
        if (triggerEntered)
        {
            doorAnimator.SetBool("door_trigger", true);
        }
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool("door_trigger", false);
    }
}
