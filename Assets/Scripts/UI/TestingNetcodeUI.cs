using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;


    private void Awake()
    {
        startHostButton.onClick.AddListener(() => {
            Debug.Log("startHost");
            NetworkManager.Singleton.StartHost();
            LobbyManager.Instance.PlayersInGame++;
            Hide();
        });

        startClientButton.onClick.AddListener(() => {
            Debug.Log("startClient");
            NetworkManager.Singleton.StartClient();
            LobbyManager.Instance.PlayersInGame++;
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
