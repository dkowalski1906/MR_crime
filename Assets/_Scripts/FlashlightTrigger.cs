using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FlashlightTrigger : MonoBehaviour
{
    [Header("Target to activate on grab")]
    public GameObject targetObject;

    private XRGrabInteractable grabInteractable;

    // Initialize target state and set up XR interaction events
    private void Start()
    {
        if (targetObject != null)
            targetObject.SetActive(false);

        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    // Activate target when flashlight is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        if (targetObject != null)
            targetObject.SetActive(true);
    }

    // Deactivate target when flashlight is released
    private void OnRelease(SelectExitEventArgs args)
    {
        if (targetObject != null)
            targetObject.SetActive(false);
    }
}
