﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerController : MonoBehaviour
{
    public Material repaired;
    double speed;
    double reverseTimer = -1;

    [Range(1, 2)]
    [Tooltip("1 = Walk forward, 2 = Walk in circles")]
    public int behaviourMode;

    int behaviour = 0;
    public Transform ButtonObjective;

    NavMeshAgent agent;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        speed = 0;
    }

    public void BeginBeingBroken()
    {
        speed = 2;
        behaviour = behaviourMode;
    }

    void Update()
    {
        LayerMask mask = LayerMask.GetMask("Obstacles");

        rb.velocity = transform.forward * (float)speed;

        if (behaviour == 1)
        {
            if (reverseTimer > 0)
            {
                reverseTimer -= Time.deltaTime;
            }
            else if (reverseTimer != -99)
            {
                speed = 2;
                reverseTimer = -99;
            }

            if (reverseTimer < 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, mask))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    speed = -1f;
                    reverseTimer = .5f;
                }
            }
        }
        if(behaviour == 2)
        {
            transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
        }
        if(behaviour == 3)
        {
            speed = 0;

            agent.SetDestination(ButtonObjective.position);

            Vector3 pos, tar;
            pos = transform.position;
            pos.y = 0;
            tar = ButtonObjective.position;
            tar.y = 0;
            
            if (Vector3.Distance(pos, tar) < 1.1f)
            {
                ButtonObjective.gameObject.GetComponent<ButtonController>().Activate();

                agent.ResetPath();
            }

            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Repair"))
        {
            GetComponent<Renderer>().material = repaired;

            behaviour = 3;
        }
    }
}
