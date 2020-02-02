using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WackEController : MonoBehaviour
{
    public static WackEController singleton;

    [TextArea(3,10)]
    public string[] lines;

    public bool speakOnStart;

    public bool spoken = false;

    private void Start()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        if (speakOnStart && !singleton.spoken)
        {
            Speak();
        }
    }

    public void Speak()
    {
        DialogueManager.singleton.gameObject.SetActive(true);
        DialogueManager.singleton.StartDialogue(lines);
        spoken = true;
    }
}
