using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MuseumManager : MonoBehaviour
{
    // Instance statique pour acc�s global (pattern Singleton)
    public static MuseumManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject bloodTextObject;
    public GameObject pictureTextObject;
    public GameObject paintTextObject;

    public GameObject confirmationText;
    public GameObject artTableObjectToHide;
    public GameObject laboratoryConfirmationButton;
    public GameObject laboratoryButton;

    // Suivi des indices
    [Header("Blood Trace")]
    public int bloodHintsFound;
    public int bloodHintsRequired;

    [Header("Footprint")]
    public int pictureHintsFound;
    public int pictureHintsRequired;

    [Header("Paint Trace")]
    public int paintHintsFound;
    public int paintHintsRequired;

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
        //son de r�ussite
        audioSourceSuccessSound = successSoundGO.GetComponent<AudioSource>();
        if (audioSourceSuccessSound == null)
            audioSourceSuccessSound = successSoundGO.AddComponent<AudioSource>();

        audioSourceSuccessSound.clip = successSound;

        //son d'ambiance de mus�e
        audioSourceMuseumSound = museumSoundGO.GetComponent<AudioSource>();
        if (audioSourceMuseumSound == null)
            audioSourceMuseumSound = museumSoundGO.AddComponent<AudioSource>();

        audioSourceMuseumSound.clip = museumSound;

        audioSourceMuseumSound.volume = 1.0f;
        audioSourceMuseumSound.loop = true;
        audioSourceMuseumSound.Play();

        //modification du hand menu
        if (bloodTextObject != null)
        {
            var text = bloodTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Traces de sang collect�es : {bloodHintsFound} / {bloodHintsRequired}";
        }

        if (pictureTextObject != null)
        {
            var text = pictureTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Traces de pas collect�es : {pictureHintsFound} / {pictureHintsRequired}";
        }

        if (paintTextObject != null)
        {
            var text = paintTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Traces de peinture collect�es : {paintHintsFound} / {paintHintsRequired}";
        }
    }


    /// <summary>
    /// Chargement de la sc�ne "Laboratory"
    /// </summary>
    public void GoToLaboratory()
    {
        if(bloodHintsFound + pictureHintsFound >= (bloodHintsRequired + pictureHintsRequired) / 2)
        {
            SceneManager.LoadScene("Laboratory");
        }
        else
        {
            if (confirmationText != null)
            {
                var text = confirmationText.GetComponent<TextMeshPro>();
                if (text != null)
                    text.text = "Vous n'avez pas assez d'indices, voulez-vous quand m�me aller au laboratoire ?";
            }
            if (laboratoryConfirmationButton != null)
            {
                laboratoryConfirmationButton.SetActive(true);
            }
            laboratoryButton.SetActive(false);
        }
    }

    /// <summary>
    /// Confirmation chargement de la sc�ne "Laboratory"
    /// </summary>
    public void GoToLaboratoryConfirmation()
    {
        SceneManager.LoadScene("Laboratory");
    }

    /// <summary>
    /// Ajoute une trace de sang trouv�e et met � jour l'UI
    /// </summary>
    public void AddBloodHint()
    {
        //son
        audioSourceSuccessSound.Play();

        //modification du hand menu
        bloodHintsFound++;

        if (bloodTextObject != null)
        {
            var text = bloodTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Traces de sang collect�es : {bloodHintsFound} / {bloodHintsRequired}";
        }
    }

    /// <summary>
    /// Ajoute une photo prise et met � jour l'UI
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
                text.text = $"Traces de pas collect�es : {pictureHintsFound} / {pictureHintsRequired}";
        }
    }

    /// <summary>
    /// Ajoute une trace de peinture et met � jour l'UI
    /// </summary>
    public void AddPaintHint()
    {
        //son
        audioSourceSuccessSound.Play();

        //modification du hand menu
        paintHintsFound++;

        if (pictureTextObject != null)
        {
            var text = paintTextObject.GetComponent<TextMeshPro>();
            if (text != null)
                text.text = $"Traces de peinture collect�es : {paintHintsFound} / {paintHintsRequired}";
        }
    }
}
