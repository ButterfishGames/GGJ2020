﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController singleton;

    public Block[] inventory;

    public int numSlots;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}