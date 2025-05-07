using System.Collections.Generic;
using UnityEngine;

public class Centrifuge : MonoBehaviour
{
    [Header("UI or GameObjects to Activate per Tube")]
    public List<GameObject> objectsToActivateFirstTube;
    public List<GameObject> objectsToActivateSecondTube;
    public List<GameObject> objectsToActivateThirdTube;

    public GameObject chromFinal;

    private int tubesProcessed = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paint"))
        {
            List<GameObject> listToActivate = null;

            switch (tubesProcessed)
            {
                case 0:
                    listToActivate = objectsToActivateFirstTube;
                    break;
                case 1:
                    listToActivate = objectsToActivateSecondTube;
                    break;
                case 2:
                    listToActivate = objectsToActivateThirdTube;
                    break;
                default:
                    Debug.Log("Tous les tubes ont déjà été traités.");
                    return;
            }

            foreach (var obj in listToActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            other.gameObject.SetActive(false);
            chromFinal.SetActive(true);
            GameManager.Instance.AddPaintAnalysis();
            tubesProcessed++;
        }
    }
}
