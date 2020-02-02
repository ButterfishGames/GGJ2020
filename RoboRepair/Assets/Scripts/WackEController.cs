using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WackEController : MonoBehaviour
{
    public string[] lines;

    public void Speak()
    {
        DialogueManager.singleton.StartDialogue(lines);
    }
}
