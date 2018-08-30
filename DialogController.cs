using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KopliSoft.Dialogs
{
    public class DialogController : MonoBehaviour
    {
        private const string INTERACTION_LAYER_NAME = "Interaction";
        [SerializeField]
        private float dialogInteractionRadius;
        private bool dialogInProgress;

        // Update is called once per frame
        void Update()
        {
            if (!dialogInProgress && Input.GetButtonDown("Interact"))
            {
                Collider[] hitColliders = Physics.OverlapSphere(
                    transform.position,
                    dialogInteractionRadius,
                    LayerMask.GetMask(INTERACTION_LAYER_NAME),
                    QueryTriggerInteraction.Collide);
                foreach (Collider collider in hitColliders)
                {
                    DialogInteractable dialogInteractable = collider.GetComponent<DialogInteractable>();
                    if (dialogInteractable != null && dialogInteractable.flowchart != null)
                    {
                        dialogInProgress = true;
                        dialogInteractable.flowchart.ExecuteBlock("Start");
                        Fungus.BlockSignals.OnBlockEnd += OnBlockEnd;
                        return;
                    }
                }
            }
        }

        void OnBlockEnd(Fungus.Block block)
        {
            if (block.BlockName.Equals("End"))
            {
                Fungus.BlockSignals.OnBlockEnd -= OnBlockEnd;
                dialogInProgress = false;
            }
        }

    }

}
