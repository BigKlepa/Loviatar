using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{

    public Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog dialogi)
    {

        Debug.Log("höpöttelyhommii" + dialogi.speakerName + dialogi.sentences);
    }

}
