using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames
{
    public class IInteractable : MonoBehaviour
    {

        [HideInInspector] public bool canInteract;

        [Header("Visual Cue")]
        [SerializeField] protected SpriteRenderer visualCuePrefab;
        [SerializeField] protected Vector3 VISUAL_CUE_OFFSET; 
        protected SpriteRenderer currentVisualCue;

        private TopDownCharacter interactingPlayer;

        protected virtual void Awake()
        {
            canInteract = true;
        }

        public virtual void OnInteract(TopDownCharacter character = null)
        {
            //Debug.Log(gameObject.name);
            if(canInteract && character != null) //&& ExplorationManager.Instance.currentInteractable == null)
            {
                //ExplorationManager.Instance.currentInteractable = this;
                character?.StopPlayerMovement();
                canInteract = false;
                interactingPlayer = character;
                StartCoroutine(Interact());
            }
        }

        protected virtual void OnUninteract()
        {
            //ExplorationManager.Instance.currentInteractable = null;
            canInteract = true;
            interactingPlayer.canMove = true;
            interactingPlayer = null;
            if(PlayerMovement.Instance != null) PlayerMovement.Instance.canMove = true;
        }

        protected virtual IEnumerator Interact()
        {
            yield return new WaitForSeconds(2f);

            OnUninteract();
            StopCoroutine(Interact());
        }
    }
}