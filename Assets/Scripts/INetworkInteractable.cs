using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class INetworkInteractable : NetworkBehaviour, IComparable<INetworkInteractable>
{
    [HideInInspector] public bool canInteract;
    [SerializeField] private int interactionPriority = 0;
    private TopDownCharacter interactingPlayer;

    [Header("Visual Cue")]
    [SerializeField] protected SpriteRenderer visualCuePrefab;
    public Vector3 VISUAL_CUE_OFFSET = Vector2.up; 
    protected SpriteRenderer currentVisualCue;

    [SerializeField] protected SpriteRenderer interactableSprite;
    private List<Color> colors = new List<Color>() {Color.white, Color.green};

    protected virtual void Awake()
    {
        canInteract = true;
    }

    public virtual void OnInteract(TopDownCharacter character = null)
    {
        Debug.Log(gameObject.name);
        if(canInteract && character != null)
        {
            //ToggleVisualCue(false);
            interactingPlayer = character;
            Interact_ServerRpc();
        }
    }

    //client is having issues calling this function
    [ServerRpc(RequireOwnership = false)]
    protected virtual void Interact_ServerRpc() => Interact_ClientRpc();

    [ClientRpc]
    protected virtual void Interact_ClientRpc()
    {
        canInteract = false;
        StartCoroutine(Interact());
    }

    [ClientRpc]
    protected virtual void OnUninteract_ClientRpc()
    {
        canInteract = true;
        interactingPlayer = null;
        if(TopDownCharacter.LocalInstance != null && TopDownCharacter.LocalInstance.interactablesInRange.Contains(this)) 
        {
            TopDownCharacter.LocalInstance.AddInteractable(this);
        }
    }

    protected virtual IEnumerator Interact()
    {
        ChangeSpriteColor_ClientRpc(1);
        yield return new WaitForSeconds(10f);
        ChangeSpriteColor_ClientRpc(0);

        OnUninteract_ClientRpc();
        StopCoroutine(Interact());
    }

    [ClientRpc]
    protected void ChangeSpriteColor_ClientRpc(int colorIndex) => interactableSprite.color = colors[colorIndex];

    // public virtual void ToggleVisualCue(bool on)
    // {
    //     if(on && visualCuePrefab != null)
    //     {
    //         currentVisualCue = Instantiate(visualCuePrefab, transform);
    //         currentVisualCue.transform.localPosition = VISUAL_CUE_OFFSET;
    //     }
    //     else if(currentVisualCue != null)
    //         Destroy(currentVisualCue.gameObject);
    // }

    public int CompareTo(INetworkInteractable other)
    {
        if(other.Equals(this)) return 0;
        int difference = other.GetInteractionPriority() - interactionPriority;
        return difference != 0 ? difference : difference + 1 ;
    }

    public virtual int GetInteractionPriority()
    {
        return interactionPriority;
    }
}