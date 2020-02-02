using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject Door;

    DoorController door;

    private void Start()
    {
        door = Door.GetComponent<DoorController>();
    }

    public void Activate()
    {
        if (door.status == 0)
        {
            door.Open();
        }
    }
}
