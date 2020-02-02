using System.Collections;
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
        Vector3 vAdjust = transform.forward;
        vAdjust.x = Mathf.RoundToInt(vAdjust.x) == 0 ? 1 : 0;
        vAdjust.y = Mathf.RoundToInt(vAdjust.y) == 0 ? 1 : 0;
        vAdjust.z = Mathf.RoundToInt(vAdjust.z) == 0 ? 1 : 0;

        Debug.Log(transform.forward);
        Debug.Log(vAdjust);

        rb.velocity = new Vector3(
            rb.velocity.x * vAdjust.x,
            rb.velocity.y * vAdjust.y,
            rb.velocity.z * vAdjust.z);
    }

    IEnumerator Turn(int amount)
    {
        rotation = new Vector3(0, turnSpeed * (amount/Mathf.Abs(amount)), 0);

        yield return new WaitForSeconds(Mathf.Abs(amount)/turnSpeed);

        rotation = Vector3.zero;
    }
}
