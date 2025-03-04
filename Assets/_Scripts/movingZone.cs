using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMovement : MonoBehaviour
{
    public float speed = 2f; // Vitesse de déplacement
    public Vector3 movementDirection = Vector3.right; // Direction du déplacement
    private Transform objectToMove; // Référence à l'objet détecté dans la zone

    private void Start()
    {
        // Vérifie les objets déjà dans la zone au lancement
        DetectObjectsInZone();
    }

    void Update()
    {
        // Déplace l'objet s'il est détecté
        if (objectToMove != null)
        {
            objectToMove.Translate(movementDirection * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Récupère l'objet qui entre dans la zone
        if (objectToMove == null) // Évite d'écraser la référence si un objet est déjà détecté
        {
            objectToMove = other.transform;
            Debug.Log($"Objet {objectToMove.name} détecté dans la zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Arrête le déplacement si l'objet sort de la zone
        if (other.transform == objectToMove)
        {
            Debug.Log($"Objet {objectToMove.name} sorti de la zone.");
            objectToMove = null;
        }
    }

    private void DetectObjectsInZone()
    {
        // Détecte les objets déjà présents dans le trigger
        Collider[] colliders = Physics.OverlapBox(transform.position, GetComponent<Collider>().bounds.extents);
        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.transform != transform)
            {
                objectToMove = collider.transform;
                Debug.Log($"Objet {objectToMove.name} détecté au démarrage.");
                return; // Ne détecte qu'un seul objet
            }
        }
    }
}
