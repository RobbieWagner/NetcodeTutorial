using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

//https://www.youtube.com/watch?v=7glCsF9fv3s 48:30 for server auth movement

public partial class TopDownCharacter : NetworkBehaviour
{

    [SerializeField] private List<INetworkInteractable> interactablesInRange;

    public void OnTriggerEnter2D(Collider2D other)
    {
        INetworkInteractable interactable = other.GetComponent(typeof(INetworkInteractable)) as INetworkInteractable;
        if(interactable != null)
        {
            Debug.Log("hi");
            interactablesInRange.Add(interactable);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        INetworkInteractable interactable = other.GetComponent(typeof(INetworkInteractable)) as INetworkInteractable;
        if(interactable != null)
        {
            interactablesInRange.Remove(interactable);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(IsOwner && interactablesInRange?.Count > 0)
        {
            interactablesInRange[0].OnInteract(this);
        }
    }
}
