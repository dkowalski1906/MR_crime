using UnityEngine;

public class Footprint : MonoBehaviour
{
    public GameObject cameraGO;  // Le GameObject contenant la caméra
    public float detectionDistance = 100f;  // Distance maximale pour la détection

    private Camera cameraComponent;  // Le composant Camera

    void Start()
    {
        // Vérifie si le GameObject est assigné et récupère le composant Camera
        if (cameraGO != null)
        {
            cameraComponent = cameraGO.GetComponent<Camera>();
        }
    }

    void Update()
    {
        // Vérifie si la caméra a été récupérée
        if (cameraComponent == null) return;

        // Effectuer un raycast à partir du centre de la caméra dans sa direction
        Ray ray = cameraComponent.ScreenPointToRay(new Vector2(cameraComponent.pixelWidth / 2, cameraComponent.pixelHeight / 2));
        RaycastHit hit;

        // Si le rayon touche un objet dans la distance de détection
        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            // Logge le nom de l'objet touché
            Debug.Log("L'objet détecté : " + hit.collider.gameObject.name);
        }
    }
}
