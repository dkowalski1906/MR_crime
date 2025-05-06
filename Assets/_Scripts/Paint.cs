using System.Collections;
using UnityEngine;

public class Paint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paint"))
        {
            // Notifie le GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddPaintClue();
                other.tag = "Untagged";
            }
        }
    }
}
