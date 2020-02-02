using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject repairShot;
    public GameObject bulletSpawn;
    public GameObject[] enemies;
    public GameObject[] workers;
    public GameObject explosion;

    [SerializeField]
    double timePassed;

    int[] commandActions;
    int[] commandValues;

    public bool executing = false;

    Vector3 rotation = Vector3.zero;

    //Just for tweaking and iterating, should not be modifiable by player
    int _moveSpeed = 5;
    int _turnSpeed = 90;

    void Start()
    {
        //SetCommands();
    }

    void Update()
    {
        if (executing)
        {
            timePassed = Time.time;
        }

        gameObject.transform.Rotate(rotation * Time.deltaTime);
    }

    public void SetCommands()
    {
        foreach (GameObject g in enemies)
        {
            g.GetComponent<EnemyController>().BeginPathing();
        }

        foreach (GameObject g in workers)
        {
            g.GetComponent<WorkerController>().BeginBeingBroken();
        }
    }

    public void SetCommands(int[] actions, int[] values)
    {
        commandActions = actions;
        commandValues = values;

        StartCoroutine(ExecuteCodeBlocks());

        foreach(GameObject g in enemies)
        {
            g.GetComponent<EnemyController>().BeginPathing();
        }
        foreach (GameObject g in workers)
        {
            g.GetComponent<WorkerController>().BeginBeingBroken();
        }
    }

    IEnumerator ExecuteCodeBlocks()
    {
        for (int i = 0; i < commandActions.Length; i++)
        {
            CheckNumber(commandActions[i], commandValues[i]);
            executing = true;

            yield return new WaitUntil(() => executing == false);

            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    IEnumerator Move(float distance)
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * _moveSpeed;

        yield return new WaitForSeconds(distance/_moveSpeed);

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        executing = false;
    }

    IEnumerator Turn(float amount)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameObject boom = Instantiate(explosion, transform.position + Vector3.up * 2, Quaternion.identity);
            boom.transform.SetParent(gameObject.transform);
            Vector3 direction = (collision.transform.position - transform.position);
            StartCoroutine(DestroySelf(boom));
            Destroy(collision.gameObject, .1f);
        }
    }

    IEnumerator DestroySelf(GameObject g)
    {
        yield return new WaitForSeconds(2);
        g.transform.SetParent(null);
        foreach (MeshRenderer m in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = false;
        }
    }
}
