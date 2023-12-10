using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

//https://www.youtube.com/watch?v=7glCsF9fv3s 48:30 for server auth movement

public partial class TopDownCharacter : NetworkBehaviour
{

    private List<IInteractable> interactablesInRange;

    public void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent(typeof(IInteractable)) as IInteractable;
        if(interactable != null)
        {
            interactablesInRange.Add(interactable);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent(typeof(IInteractable)) as IInteractable;
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
