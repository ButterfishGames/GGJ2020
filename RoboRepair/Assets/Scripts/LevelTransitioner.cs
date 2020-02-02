using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitioner : MonoBehaviour
{
    public int nextLevelIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MenuController.singleton != null)
            {
                Destroy(MenuController.singleton.gameObject);
            }

            if (WackEController.singleton != null)
            {
                Destroy(WackEController.singleton.gameObject);
            }

            LevelLoader.singleton.LoadScene(nextLevelIndex);
        }
    }
}
