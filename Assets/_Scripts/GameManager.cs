using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Instance statique pour accès global (pattern Singleton)
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject fingerprintTextObject;
    public GameObject pictureTextObject;
    public GameObject artTableObjectToHide;

    // Suivi des indices (empreintes)
    private int fingerprintHintsFound = 0;
    private int fingerprintHintsRequired = 3;

    // Suivi des indices (photos)
    private int pictureHintsFound = 0;
    private int pictureHintsRequired = 4;

    private void Awake()
    {
        // Assure qu'un seul GameManager existe (Singleton)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Tente de charger la scène "Laboratory" si toutes les preuves sont récupérées
    /// </summary>
    public void GoToLaboratory()
    {
        if (fingerprintHintsFound >= fingerprintHintsRequired && pictureHintsFound >= pictureHintsRequired)
        {
            SceneManager.LoadScene("Laboratory");
        }
        else
        {
            Debug.Log("Accès refusé : toutes les preuves ne sont pas encore réunies.");
        }
    }

    /// <summary>
    /// Ajoute une empreinte trouvée et met à jour l'UI
    /// </summary>
    public void AddFingerprintHint()
    {
        fingerprintHintsFound++;

        if (fingerprintTextObject != null)
        {
            var text = fingerprintTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Fingerprints collected: {fingerprintHintsFound} / {fingerprintHintsRequired}";
        }
    }

    /// <summary>
    /// Ajoute une photo prise et met à jour l'UI
    /// </summary>
    public void AddPictureHint()
    {
        pictureHintsFound++;

        if (pictureTextObject != null)
        {
            var text = pictureTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Pictures taken: {pictureHintsFound} / {pictureHintsRequired}";
        }
    }
}
