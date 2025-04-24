using UnityEngine;

public class Fingerprint : MonoBehaviour
{
    private bool isDetected = false;

    [Header("Material to apply when detected")]
    public Material detectedMaterial;

    private Renderer fingerprintRenderer;

    private void Start()
    {
        // Cache le Renderer pour éviter de l'appeler à chaque frame
        fingerprintRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Si l'empreinte est détectée, applique le matériau
        if (isDetected && fingerprintRenderer != null && detectedMaterial != null)
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
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddFingerprintHint();
            }
        }
    }
}
