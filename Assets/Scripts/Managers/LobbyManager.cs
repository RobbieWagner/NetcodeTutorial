using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LobbyManager : NetworkBehaviour
{

    public static LobbyManager Instance {get; private set;}

    private int playersInGame;
    public int PlayersInGame
    {
        get
        {
            return playersInGame;
        }
        set
        {
            if(playersInGame == value) return;
            playersInGame = value;
            OnPlayerCountChanged?.Invoke(playersInGame);
        }
    }
    public delegate void OnPlayerCountChangedDelegate(int players);
    public event OnPlayerCountChangedDelegate OnPlayerCountChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
        playersInGame = 0;        
    }


}
