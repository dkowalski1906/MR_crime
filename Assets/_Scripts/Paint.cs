using System.Collections;
using UnityEngine;

public class Paint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Paint"))
        {
            // Notifie le GameManager
            if (MuseumManager.Instance != null)
            {
                MuseumManager.Instance.AddPaintHint();
                other.tag = "Untagged";
            }
        }
    }
}
