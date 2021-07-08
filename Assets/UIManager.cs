using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject container_NPC;
    public GameObject container_Player;
    public Text text_NPC;
    public Text[] text_Choices;
    public bool freezePlayer = false;
    public PlayerController player;
        // Start is called before the first frame update
    void Start()
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!VD.isActive)
            {
                Begin();
            }
            else
            {
                VD.Next();
            }
        }
    }
    void Begin()
    {
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += End;
        VD.BeginDialogue(player.inTrigger);
        player.setFrozen(true);
    }

    void UpdateUI(VD.NodeData data)
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        if (data.isPlayer)
        {
            container_Player.SetActive(true);
        }
        else
        {
            container_NPC.SetActive(true);
            text_NPC.text = data.comments[data.commentIndex];
        }
    }
    void End(VD.NodeData data)
    {
        container_NPC.SetActive(false);
        container_Player.SetActive(false);
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue();
        player.setFrozen(false);
    }

    void OnDisable()
    {

        if (container_NPC != null)
        {
            End(null);
        }
    }
}
