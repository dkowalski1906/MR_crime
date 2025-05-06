using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CameraInteraction : MonoBehaviour
{
    [Header("Camera Objects")]
    public GameObject cameraObject;
    public GameObject cameraFlash;
    public GameObject cameraScreen;

    private XRGrabInteractable cameraInteractable;
    private Camera cameraView;

    private const string TAG_PIC_TRIGGER = "PicTrigger";
    private const string TAG_PIC_BUTTON = "PicButton";
    private const string TAG_CAMERA_BUTTON = "CameraButton";
    private const string TAG_FOOTPRINT = "Footprint";

    private static bool isCameraMode = true;

    [Header("Footprint Detection")]
    public float detectionDistance = 10000f;

    [Header("Material to apply when detected")]
    public Material detectedMaterial;

    [Header("Photo Sound")]
    public GameObject cameraGO;
    public AudioClip cameraSound;
    private AudioSource audioSourceCameraSound;

    private void Start()
    {
        if (cameraObject != null)
            cameraInteractable = cameraObject.GetComponent<XRGrabInteractable>();

        if (cameraScreen != null)
            cameraView = cameraScreen.GetComponent<Camera>();

        if (cameraFlash != null)
            cameraFlash.SetActive(false);

        //son de photo
        audioSourceCameraSound = cameraGO.GetComponent<AudioSource>();
        if (audioSourceCameraSound == null)
            audioSourceCameraSound = cameraGO.AddComponent<AudioSource>();
        audioSourceCameraSound.clip = cameraSound;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TAG_PIC_TRIGGER) || cameraInteractable == null || !cameraInteractable.isSelected)
            return;

        switch (gameObject.tag)
        {
            case TAG_PIC_BUTTON:

                if (isCameraMode)
                {
                    cameraView.enabled = false;
                    isCameraMode = false;
                    audioSourceCameraSound.Play();
                    StartCoroutine(ActivateFlashTemporarily(0.2f));
                    DetectFootprints();
                }
                break;

            case TAG_CAMERA_BUTTON:

                if (!isCameraMode)
                {
                    cameraView.enabled = true;
                    isCameraMode = true;
                }
                break;
        }
    }

    private void DetectFootprints()
    {
        if (cameraView == null) return;

        Ray ray = cameraView.ScreenPointToRay(new Vector2(cameraView.pixelWidth / 2, cameraView.pixelHeight / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(TAG_FOOTPRINT))
            {
                // Appliquer le matériau
                Renderer renderer = hitObject.GetComponent<Renderer>();
                if (renderer != null && detectedMaterial != null)
                {
                    renderer.material = detectedMaterial;
                }

                // Notifie le GameManager
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddFootprintClue();
                }

                hitObject.tag = "Untagged";
            }
        }
    }

    private IEnumerator ActivateFlashTemporarily(float duration)
    {
        if (cameraFlash == null) yield break;

        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(duration);
        cameraFlash.SetActive(false);
    }
}
