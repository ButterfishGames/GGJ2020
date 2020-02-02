using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeleter : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 6);
    }
}
