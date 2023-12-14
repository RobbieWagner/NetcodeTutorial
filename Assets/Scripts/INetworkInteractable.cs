using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class INetworkInteractable : NetworkBehaviour
{
    [HideInInspector] public bool canInteract;

    [Header("Visual Cue")]
    [SerializeField] protected SpriteRenderer visualCuePrefab;
    [SerializeField] protected Vector3 VISUAL_CUE_OFFSET; 
    protected SpriteRenderer currentVisualCue;
    [SerializeField] protected SpriteRenderer interactableSprite;
    private List<Color> colors = new List<Color>() {Color.white, Color.green};

    private TopDownCharacter interactingPlayer;

    protected virtual void Awake()
    {
        canInteract = true;
    }

    public virtual void OnInteract(TopDownCharacter character = null)
    {
        Debug.Log(gameObject.name);
        if(canInteract && character != null)
        {
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
}