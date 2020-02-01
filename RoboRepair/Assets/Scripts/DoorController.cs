using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int status = 0;

    Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    public void Open()
    {
        if (status != 1)
        {
            status = 1;
            transform.position = startingPosition += transform.forward * -3;
        }
    }
}
