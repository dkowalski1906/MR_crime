using System.Collections;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private bool isDetected = false;
    private bool isDetectedRed = false;

    [Header("Material to apply when detected")]
    public Material detectedMaterial;
    public Material detectedMaterialSpecial;

    private Renderer bloodRenderer;

    public float waitTime;

    private void Start()
    {
        bloodRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (isDetected && bloodRenderer != null && detectedMaterial != null && !isDetectedRed)
        {
            bloodRenderer.material = detectedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDetected && other.CompareTag("light"))
        {
            isDetected = true;

            // Notifie le GameManager
            if (MuseumManager.Instance != null)
            {
                MuseumManager.Instance.AddBloodHint();
            }

            //Red render
            StartCoroutine(HandleRedRender());
        }
    }

    IEnumerator HandleRedRender()
    {
        isDetectedRed = true;
        bloodRenderer.material = detectedMaterialSpecial;
        yield return new WaitForSeconds(waitTime);
        isDetectedRed = false;
    }
}
