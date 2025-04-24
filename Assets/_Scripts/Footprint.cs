using UnityEngine;

public class Footprint : MonoBehaviour
{
    public GameObject cameraGO;  // Le GameObject contenant la cam�ra
    public float detectionDistance = 100f;  // Distance maximale pour la d�tection

    private Camera cameraComponent;  // Le composant Camera

    void Start()
    {
        // V�rifie si le GameObject est assign� et r�cup�re le composant Camera
        if (cameraGO != null)
        {
            cameraComponent = cameraGO.GetComponent<Camera>();
        }
    }

    void Update()
    {
        // V�rifie si la cam�ra a �t� r�cup�r�e
        if (cameraComponent == null) return;

        // Effectuer un raycast � partir du centre de la cam�ra dans sa direction
        Ray ray = cameraComponent.ScreenPointToRay(new Vector2(cameraComponent.pixelWidth / 2, cameraComponent.pixelHeight / 2));
        RaycastHit hit;

        // Si le rayon touche un objet dans la distance de d�tection
        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            // Logge le nom de l'objet touch�
            Debug.Log("L'objet d�tect� : " + hit.collider.gameObject.name);
        }
    }
}
