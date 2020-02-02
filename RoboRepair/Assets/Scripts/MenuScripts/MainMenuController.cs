using System.Collections;
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

    private bool run;

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

        run = false;
    }

    public void RunScript()
    {
        if (slot.block == null)
        {
            return;
        }

        if (run)
        {
            return;
        }

        switch (slot.block.GetComponent<MenuBlockController>().blockType)
        {
            case MenuBlockController.MenuBlockType.play:
                LevelLoader.singleton.LoadScene(levelOneBuildIndex);
                run = true;
                break;

            case MenuBlockController.MenuBlockType.credits:
                LevelLoader.singleton.LoadScene(creditsBuildIndex);
                run = true;
                break;

            case MenuBlockController.MenuBlockType.quit:
                Application.Quit();
                run = true;
                break;

            default:
                Debug.Log("ERROR: Invalid MenuBlockType");
                break;
        }
    }
}
