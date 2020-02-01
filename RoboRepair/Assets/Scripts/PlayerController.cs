using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject repairShot;
    public GameObject bulletSpawn;

    [SerializeField]
    double timePassed;

    int[] commandActions;
    int[] commandValues;

    bool executing = false;

    int speed = 0;
    Vector3 rotation = Vector3.zero;

    //Just for tweaking and iterating, should not be modifiable by player
    int _moveSpeed = 5;
    int _turnSpeed = 45;

    void Start()
    {
        
    }

    void Update()
    {
        if (executing)
        {
            timePassed = Time.time;
        }

        gameObject.transform.Translate(speed * Vector3.forward * Time.deltaTime);
        gameObject.transform.Rotate(rotation * Time.deltaTime);
    }

    public void SetCommands(int[] actions, int[] values)
    {
        commandActions = actions;
        commandValues = values;

        StartCoroutine(ExecuteCodeBlocks());
    }

    IEnumerator ExecuteCodeBlocks()
    {
        for (int i = 0; i < commandActions.Length; i++)
        {
            CheckNumber(commandActions[i], commandValues[i]);
            executing = true;

            yield return new WaitUntil(() => executing == false);
        }
    }

    void CheckNumber(int action, int value)
    {
        switch (action)
        {
            //MOVE
            case 0:
                StartCoroutine(Move(value));
                break;
            //TURN
            case 1:
                StartCoroutine(Turn(value));
                break;
            //SHOOT
            case 2:
                StartCoroutine(Shoot());
                break;
            //REPAIR
            case 3:
                StartCoroutine(Repair());
                break;
            //WAIT
            case 4:
                StartCoroutine(Wait(value));
                break;
        }
    }

    IEnumerator Move(int distance)
    {
        speed = _moveSpeed;

        yield return new WaitForSeconds(distance/_moveSpeed);

        speed = 0;
        executing = false;
    }

    IEnumerator Turn(int amount)
    {
        rotation = new Vector3(0, _turnSpeed * (amount/Mathf.Abs(amount)), 0);

        yield return new WaitForSeconds(Mathf.Abs(amount)/_turnSpeed);

        rotation = Vector3.zero;
        executing = false;
    }

    IEnumerator Shoot()
    {
        GameObject firedShot = Instantiate(bullet, bulletSpawn.transform.position, transform.rotation);
        firedShot.GetComponent<Rigidbody>().velocity = transform.forward * 10;

        yield return new WaitForSeconds(1);

        executing = false;
        Destroy(firedShot);
    }

    IEnumerator Repair()
    {
        GameObject firedShot = Instantiate(repairShot, bulletSpawn.transform.position, transform.rotation);
        firedShot.GetComponent<Rigidbody>().velocity = transform.forward * 10;

        yield return new WaitForSeconds(1);

        executing = false;
        Destroy(firedShot);
    }

    IEnumerator Wait(int time)
    {
        yield return new WaitForSeconds(time);

        executing = false;
    }
}
