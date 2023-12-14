using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class LobbyHUD : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<Canvas> canvas;
    [SerializeField] private TextMeshProUGUI testText;

    private void Awake()
    {
        LobbyManager.Instance.OnPlayerCountChanged += UpdateTestText;
    }

    private void UpdateTestText(int players)
    {
        if(!IsServer) return;

        for(int i = 0; i < players; i++)
        {
            testText.text += i + " ";
        }
    }
}
