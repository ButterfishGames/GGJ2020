﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    public enum ConveyorType
    {
        straight,
        leftTurn,
        rightTurn
    }

    public ConveyorType conveyorType;

    public float moveSpeed;

    public float turnSpeed;

    private Vector3 rotation = Vector3.zero;

    private Transform targetTransform;

    private bool vAdded = false;

    private void Update()
    {
        if (conveyorType != ConveyorType.straight && targetTransform != null)
        {
            targetTransform.Rotate(rotation * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (conveyorType != ConveyorType.straight)
        {
            targetTransform = other.transform;
            int mod = conveyorType == ConveyorType.rightTurn ? 1 : -1;
            StartCoroutine(Turn(90 * mod));
        }
        
        rb.velocity = (rb.velocity + transform.forward * 0.001f).normalized * (transform.forward * moveSpeed).magnitude;
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb.velocity == Vector3.zero)
        {
            rb.velocity = transform.forward * moveSpeed;
        }
        else
        {
            rb.velocity = (rb.velocity + transform.forward * 0.001f).normalized * (transform.forward * moveSpeed).magnitude;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    IEnumerator Turn(int amount)
    {
        rotation = new Vector3(0, turnSpeed * (amount/Mathf.Abs(amount)), 0);

        yield return new WaitForSeconds(Mathf.Abs(amount)/turnSpeed);

        rotation = Vector3.zero;
    }
}
