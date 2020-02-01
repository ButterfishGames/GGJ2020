using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public BoxCollider visionCollider;
    RaycastHit hit;
    int currentWP = 0;
    bool attackingPlayer = false;
    double shootTimer;
    double shootCooldown = 2;

    public GameObject WaypointsObject;
    public GameObject Player;
    public GameObject bullet;
    public GameObject bulletSpawn;

    List<Transform> Waypoints;

    NavMeshAgent agent;

    void Start()
    {
        WaypointsObject.transform.SetParent(null);
        Waypoints = new List<Transform>();
        agent = GetComponent<NavMeshAgent>();
        Transform[] children = WaypointsObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if(child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            if (child.gameObject.CompareTag("Waypoint"))
            {
                Waypoints.Add(child);
            }
        }

        
    }

    public void BeginPathing()
    {
        agent.destination = Waypoints[0].position;
    }

    void Update()
    {
        bool coneIntersected = Physics.BoxCast(visionCollider.bounds.center, visionCollider.bounds.size / 2, visionCollider.gameObject.transform.forward, out hit, visionCollider.gameObject.transform.rotation, 1);

        if(coneIntersected)
        {
            if(hit.transform.gameObject.GetComponentInParent<PlayerController>())
            {
                agent.ResetPath();

                if (!attackingPlayer)
                {
                    attackingPlayer = true;
                    shootTimer = shootCooldown;
                }
            }
        }

        if(attackingPlayer)
        {
            transform.LookAt(Player.transform);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);

            if(Vector3.Distance(transform.position, Player.transform.position) > 6)
            {
                agent.SetDestination(Player.transform.position);
            }
            else
            {
                agent.ResetPath();
            }

            if(shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            else
            {
                GameObject firedShot = Instantiate(bullet, bulletSpawn.transform.position, transform.rotation);
                firedShot.GetComponent<Rigidbody>().velocity = transform.forward * 10;
                Destroy(firedShot, 1);

                shootTimer = shootCooldown;
            }

        }


        if (Vector3.Distance(transform.position, Waypoints[currentWP].position) < .1f)
        {
            transform.position = Waypoints[currentWP].position;
            

            if (currentWP == (Waypoints.Count - 1))
            {
                currentWP = 0;
            }
            else
            {
                currentWP += 1;
            }

            StartCoroutine(NewDestination());
        }
    }

    IEnumerator NewDestination()
    {
        yield return new WaitForSeconds(1);

        agent.destination = Waypoints[currentWP].position;
    }
}
