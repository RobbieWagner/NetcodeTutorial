using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class TopDownCharacter : MonoBehaviour
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
        if(interactablesInRange?.Count > 0)
        {
            interactablesInRange[0].OnInteract(this);
        }
    }
}
