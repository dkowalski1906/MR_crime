using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MuseumManager : MonoBehaviour
{
    // Instance statique pour accès global (pattern Singleton)
    public static MuseumManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject fingerprintTextObject;
    public GameObject pictureTextObject;
    public GameObject confirmationText;
    public GameObject artTableObjectToHide;
    public GameObject laboratoryConfirmationButton;
    public GameObject laboratoryButton;

    // Suivi des indices (empreintes)
    [Header("Fingerprints")]
    public int fingerprintHintsFound;
    public int fingerprintHintsRequired;

    // Suivi des indices (photos)
    [Header("Footprints")]
    public int pictureHintsFound;
    public int pictureHintsRequired;

    //Son
    [Header("Success Sound")]
    public GameObject successSoundGO;
    public AudioClip successSound;
    private AudioSource audioSourceSuccessSound;

    [Header("Museum Sound")]
    public GameObject museumSoundGO;
    public AudioClip museumSound;
    private AudioSource audioSourceMuseumSound;

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

    private void Start()
    {
        //son de réussite
        audioSourceSuccessSound = successSoundGO.GetComponent<AudioSource>();
        if (audioSourceSuccessSound == null)
            audioSourceSuccessSound = successSoundGO.AddComponent<AudioSource>();

        audioSourceSuccessSound.clip = successSound;

        //son d'ambiance de musée
        audioSourceMuseumSound = museumSoundGO.GetComponent<AudioSource>();
        if (audioSourceMuseumSound == null)
            audioSourceMuseumSound = museumSoundGO.AddComponent<AudioSource>();

        audioSourceMuseumSound.clip = museumSound;

        audioSourceMuseumSound.volume = 1.0f;
        audioSourceMuseumSound.loop = true;
        audioSourceMuseumSound.Play();
    }


    /// <summary>
    /// Chargement de la scène "Laboratory"
    /// </summary>
    public void GoToLaboratory()
    {
        Debug.Log(fingerprintHintsFound + pictureHintsFound);
        if(fingerprintHintsFound + pictureHintsFound >= (fingerprintHintsRequired + pictureHintsRequired) / 2)
        {
            SceneManager.LoadScene("Laboratory");
        }
        else
        {
            if (confirmationText != null)
            {
                var text = confirmationText.GetComponent<TextMeshPro>();
                if (text != null)
                    text.text = "Vous n'avez pas assez d'indices, voulez-vous quand même aller au laboratoire ?";
            }
            if (laboratoryConfirmationButton != null)
            {
                laboratoryConfirmationButton.SetActive(true);
            }
            laboratoryButton.SetActive(false);
        }
    }

    /// <summary>
    /// Confirmation chargement de la scène "Laboratory"
    /// </summary>
    public void GoToLaboratoryConfirmation()
    {
        SceneManager.LoadScene("Laboratory");
    }

    /// <summary>
    /// Ajoute une empreinte trouvée et met à jour l'UI
    /// </summary>
    public void AddFingerprintHint()
    {
        //son
        audioSourceSuccessSound.Play();

        //modification du hand menu
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
        //son
        audioSourceSuccessSound.Play();

        //modification du hand menu
        pictureHintsFound++;

        if (pictureTextObject != null)
        {
            var text = pictureTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Pictures taken: {pictureHintsFound} / {pictureHintsRequired}";
        }
    }
}
