using System.Collections;
using UnityEngine;

public class Fingerprint : MonoBehaviour
{
    private bool isDetected = false;
    private bool isDetectedRed = false;

    [Header("Material to apply when detected")]
    public Material detectedMaterial;
    public Material detectedMaterialRed;

    private Renderer fingerprintRenderer;

    public float waitTime;

    private void Start()
    {
        // Cache le Renderer pour éviter de l'appeler à chaque frame
        fingerprintRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Si l'empreinte est détectée, applique le matériau
        if (isDetected && fingerprintRenderer != null && detectedMaterial != null && !isDetectedRed)
        {
            fingerprintRenderer.material = detectedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(isDetected);

        // Si une lumière entre en collision et que ce n'est pas encore détecté
        if (!isDetected && other.CompareTag("light"))
        {
            isDetected = true;

            // Notifie le GameManager
            if (MuseumManager.Instance != null)
            {
                MuseumManager.Instance.AddFingerprintHint();
            }

            //Red render
            StartCoroutine(HandleRedRender());
        }
    }

    IEnumerator HandleRedRender()
    {
        isDetectedRed = true;
        fingerprintRenderer.material = detectedMaterialRed;
        yield return new WaitForSeconds(waitTime);
        isDetectedRed = false;
    }
}
