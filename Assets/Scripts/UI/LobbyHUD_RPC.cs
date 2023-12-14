using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class LobbyHUD_RPC : NetworkBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI testText;

    private void Awake()
    {
        LobbyManager.Instance.OnPlayerCountChanged += UpdateTestText;
    }

    private void UpdateTestText(int players)
    {
        if(!IsServer) return;

        string text = "";

        for(int i = 0; i < players; i++)
        {
            text += i + " ";
        }

        StartCoroutine(Delay(text));
    }

    private IEnumerator Delay(string text)
    {
        yield return new WaitForSeconds(5f);

        UpdateTestText_ClientRpc(text);

        StopCoroutine(Delay(text));
    }

    [ClientRpc]
    private void UpdateTestText_ClientRpc(string text)
    {
        testText.text = text;
    }
}
