using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PlayerHUD : NetworkBehaviour
{
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private Canvas screenSpaceCanvas;

    public static PlayerHUD LocalInstance {get;set;}

    [SerializeField] private Image interactionCue;
    [SerializeField] private PositionConstraint interactionCuePositionConstraint;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            LocalInstance = this;
        }
    }

    public void MoveInteractionCue(Vector2 position)
    {
        interactionCuePositionConstraint.locked = false;
        //interactionCuePositionConstraint.constraintActive = true;
        interactionCue.transform.position = position;
        ToggleCue(true);
        interactionCuePositionConstraint.locked = true;
    }

    public void ToggleCue(bool on = false)
    {
        interactionCue.enabled = on;
    }
}
