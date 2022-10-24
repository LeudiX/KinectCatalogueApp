using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDialogue_Trigger : MonoBehaviour
{
    public HelpDialogue dialogue;


public void TriggerDialogue()
{
    FindObjectOfType<HelpManager>().StartHelpDialogue(dialogue);
}

}
