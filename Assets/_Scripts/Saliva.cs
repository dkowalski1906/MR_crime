using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Saliva : MonoBehaviour
{
    [Header("Matériau à appliquer lors de la détection")]
    public Material detectedMaterial;

    private Renderer salivaRenderer;

    [Header("Son de la lampe UV")]
    public GameObject lampGO;
    public AudioClip lampSound;
    private AudioSource audioSourceLamp;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        // Préparation du son
        if (lampGO != null)
        {
            audioSourceLamp = lampGO.GetComponent<AudioSource>();
            if (audioSourceLamp == null)
                audioSourceLamp = lampGO.AddComponent<AudioSource>();

            audioSourceLamp.clip = lampSound;
        }

        // Préparation de l'interaction XR
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
        else
        {
            Debug.LogWarning("XRGrabInteractable manquant sur " + gameObject.name);
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (audioSourceLamp != null && !audioSourceLamp.isPlaying)
        {
            audioSourceLamp.Play();
        }
    }

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
