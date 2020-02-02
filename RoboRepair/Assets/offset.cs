using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offset : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Animator>().SetFloat("offset", Random.Range(.85f, 1.15f));
    }
}
