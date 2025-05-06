using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Saliva : MonoBehaviour
{
    [Header("Material to apply when detected")]
    public Material detectedMaterial;

    [Header("Sound of UV lamp")]
    public GameObject lampGO;
    public AudioClip lampSound;

    private Renderer salivaRenderer;
    private AudioSource audioSourceLamp;
    private XRGrabInteractable grabInteractable;

    // Initialize the audio source and XR grab interaction
    private void Awake()
    {
        if (lampGO != null && lampSound != null)
        {
            audioSourceLamp = lampGO.GetComponent<AudioSource>();
            if (audioSourceLamp == null)
                audioSourceLamp = lampGO.AddComponent<AudioSource>();

            audioSourceLamp.clip = lampSound;
            audioSourceLamp.playOnAwake = false;
        }

        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
        else
        {
            Debug.LogWarning("XRGrabInteractable missing on " + gameObject.name);
        }
    }

    // Clean up event listener when object is destroyed
    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }

    // Play UV lamp sound when the object is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        if (audioSourceLamp != null && !audioSourceLamp.isPlaying)
        {
            audioSourceLamp.Play();
        }
    }

    // Detect saliva object, mark it as found, and change its material
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saliva"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddSalivaClue();
                other.tag = "Untagged";

                salivaRenderer = other.GetComponent<Renderer>();
                if (salivaRenderer != null && detectedMaterial != null)
                {
                    salivaRenderer.material = detectedMaterial;
                }
            }
        }
    }
}
