using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3.0f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            NoteItem note = hit.collider.GetComponent<NoteItem>();
            if (note != null)
            {
                note.CollectNote();
            }
        }
    }
}