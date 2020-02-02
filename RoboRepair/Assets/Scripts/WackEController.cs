using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WackEController : MonoBehaviour
{
    [TextArea(3,10)]
    public string[] lines;

    public void Speak()
    {
        DialogueManager.singleton.gameObject.SetActive(true);
        DialogueManager.singleton.StartDialogue(lines);
    }
}
