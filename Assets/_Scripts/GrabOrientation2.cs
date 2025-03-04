using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetRotationOnGrab : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Ajouter l'événement sur le grab
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Récupérer le transform du contrôleur qui a grabé l'objet
        Transform controllerTransform = args.interactorObject.transform;

        // Définir l'orientation pour que l'objet soit devant le joueur
        Vector3 forwardDirection = controllerTransform.forward; // La direction devant le contrôleur
        forwardDirection.y = 0; // On ignore l'inclinaison vers le haut/bas

        // Appliquer la rotation pour orienter l'objet dans cette direction
        transform.rotation = Quaternion.LookRotation(forwardDirection);
    }
}
