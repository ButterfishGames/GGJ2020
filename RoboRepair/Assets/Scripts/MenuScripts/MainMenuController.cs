﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController singleton;

    public int levelOneBuildIndex;

    public int creditsBuildIndex;

    public Transform canvas;

    public Transform inventoryContent;

    public MenuSlotController slot;

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
    }

    public void RunScript()
    {
        if (slot.block == null)
        {
            return;
        }

        switch (slot.block.GetComponent<MenuBlockController>().blockType)
        {
            case MenuBlockController.MenuBlockType.play:
                Play();
                break;

            case MenuBlockController.MenuBlockType.credits:
                RollCredits();
                break;

            case MenuBlockController.MenuBlockType.quit:
                Application.Quit();
                break;

            default:
                Debug.Log("ERROR: Invalid MenuBlockType");
                break;
        }
    }

    private void Play()
    {
        Debug.Log("This would play the game");
        // SceneManager.LoadScene(levelOneBuildIndex);
    }

    private void RollCredits()
    {
        Debug.Log("This would roll credits");
        // SceneManager.LoadScene(creditsBuildIndex);
    }
}