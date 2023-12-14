using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System;
using System.Linq;

//https://www.youtube.com/watch?v=7glCsF9fv3s 48:30 for server auth movement

public partial class TopDownCharacter : NetworkBehaviour
{

    [HideInInspector] public SortedSet<INetworkInteractable> interactablesInRange = new SortedSet<INetworkInteractable>();
    [HideInInspector] public SortedSet<INetworkInteractable> interactableInteractables = new SortedSet<INetworkInteractable>();

    public void OnTriggerEnter2D(Collider2D other)
    {
        INetworkInteractable interactable = other.GetComponent(typeof(INetworkInteractable)) as INetworkInteractable;
        if(interactable != null)
        {
            Debug.Log("hi");
            AddInteractable(interactable);
            ShowCurrentInteractable();
        }
    }

    public void AddInteractable(INetworkInteractable interactable)
    {
        interactablesInRange.Add(interactable);
        if(interactable.canInteract) 
        {
            if(interactableInteractables.Add(interactable))
                ShowCurrentInteractable();
        }

        Debug.Log("interactables " + interactableInteractables.Count);
        Debug.Log("in range " + interactablesInRange.Count);
    }

    private void ShowCurrentInteractable()
    {
        PlayerHUD.LocalInstance?.ToggleCue();
        if(interactableInteractables?.Count > 0)
        {
            PlayerHUD.LocalInstance?.MoveInteractionCue(interactableInteractables.ElementAt(0).VISUAL_CUE_OFFSET + interactableInteractables.ElementAt(0).transform.position);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        INetworkInteractable interactable = other.GetComponent(typeof(INetworkInteractable)) as INetworkInteractable;
        if(interactable != null)
        {
            RemoveInteractable(interactable);
        }
    }

    private void RemoveInteractable(INetworkInteractable interactable)
    {
        interactablesInRange.Remove(interactable);
        if(interactableInteractables.Remove(interactable))
        {
            ShowCurrentInteractable();
            //interactable.ToggleVisualCue(false);
        }
        
        Debug.Log("interactables " + interactableInteractables.Count);
        Debug.Log("in range " + interactablesInRange.Count);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(IsOwner && interactablesInRange?.Count > 0)
        {
            interactableInteractables.ElementAt(0).OnInteract(this);
            interactableInteractables.Remove(interactableInteractables.ElementAt(0));
            ShowCurrentInteractable();
        }
    }
}
