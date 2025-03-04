using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalColor : MonoBehaviour
{
    string color = "blue";

    public void SaveData(string colorInput)
    {
        color = colorInput;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "tête")
        {
            Transform head = other.transform;
            if (head == null)
            {
                Debug.LogError("La tête n'a pas été trouvée dans le robot.");
                return;
            }

            Renderer headRenderer = head.GetComponent<Renderer>();
            if (headRenderer == null)
            {
                Debug.LogError("Le Renderer de la tête est manquant.");
                return;
            }

            switch (color)
            {
                case "blue":
                    headRenderer.material.color = Color.blue;
                    Debug.Log("Couleur appliquée : Bleu");
                    break;
                case "red":
                    headRenderer.material.color = Color.red;
                    Debug.Log("Couleur appliquée : Rouge");
                    break;
                case "green":
                    headRenderer.material.color = new Color(0.0f, 0.39f, 0.0f);
                    Debug.Log("Couleur appliquée : Vert");
                    break;
                case "yellow":
                    headRenderer.material.color = Color.yellow;
                    Debug.Log("Couleur appliquée : Jaune");
                    break;
                default:
                    Debug.LogError("Couleur non reconnue : " + color);
                    break;
            }
        }        
    }
}
