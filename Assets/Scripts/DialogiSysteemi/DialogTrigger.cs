using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{

    public Dialog dialogi;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialogi);

    }
}
