using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Material angry;
    public GameObject light;
    RaycastHit hit;
    int currentWP = 0;
    bool attackingPlayer = false;
    double shootTimer;
    double shootCooldown = 2;

    public GameObject WaypointsObject;
    public GameObject Player;
    public GameObject bullet;
    public GameObject bulletSpawn;

    public GameObject explosion;

    List<Transform> Waypoints;

    NavMeshAgent agent;

    Rigidbody rb;

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

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void BeginPathing()
    {
        agent.destination = Waypoints[0].position;
    }

    void Update()
    {
        
        bool coneIntersected = Physics.SphereCast(transform.position, 1, gameObject.transform.forward, out hit, 7);

        if(coneIntersected)
        {
            if(hit.transform.gameObject.GetComponentInParent<PlayerController>())
            {
                agent.ResetPath();

                if (!attackingPlayer && rb.isKinematic)
                {
                    attackingPlayer = true;

                    light.GetComponent<Renderer>().material = angry;
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

        if (Waypoints.ToArray().Length > 0 && Waypoints[currentWP] != null)
        {
            if (Vector3.Distance(transform.position, Waypoints[currentWP].position) < 1f)
            {
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
    }

    IEnumerator NewDestination()
    {
        yield return new WaitForSeconds(1);

        agent.destination = Waypoints[currentWP].position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            GameObject boom = Instantiate(explosion, transform.position+Vector3.up*2, Quaternion.identity);
            boom.transform.SetParent(gameObject.transform);
            agent.ResetPath();
            rb.isKinematic = false;

            //Vector3 direction = (collision.transform.position - transform.position);
            //rb.AddExplosionForce(650, (transform.position - Player.transform.position).normalized*.5f, 100, 75);
            //rb.AddForceAtPosition(direction.normalized * 200, transform.position - Vector3.down*2 - Vector3.back * 2);

            MeshCollider[] children = GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider child in children)
            {
                    child.gameObject.GetComponent<MeshCollider>().enabled = false;
            }

            agent.enabled = false;
            //gameObject.GetComponent<CapsuleCollider>().enabled = false;

            StartCoroutine(DestroySelf(boom));
            Destroy(collision.gameObject, .1f);
        }
    }

    IEnumerator DestroySelf(GameObject g)
    {
        yield return new WaitForSeconds(2);
        g.transform.SetParent(null);
        g.transform.rotation = Quaternion.identity;
        Destroy(gameObject);
    }
}
