using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FlashlightTrigger : MonoBehaviour
{
    [Header("Target to activate on grab")]
    public GameObject targetObject;

    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        if (targetObject != null)
            targetObject.SetActive(false);

        // Récupère le composant XRGrabInteractable sur l'objet
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    /// <summary>
    /// Appelé lorsque la lampe est prise en main
    /// </summary>
    private void OnGrab(SelectEnterEventArgs args)
    {
        if (targetObject != null)
            targetObject.SetActive(true);
    }

    /// <summary>
    /// Appelé lorsque la lampe est relâchée
    /// </summary>
    private void OnRelease(SelectExitEventArgs args)
    {
        if (targetObject != null)
            targetObject.SetActive(false);
    }
}
