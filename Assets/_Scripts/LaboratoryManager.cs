using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaboratoryManager : MonoBehaviour
{
    public static LaboratoryManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject bloodTextObject;
    public GameObject paintTextObject;

    [Header("Suspect GameObjects")]
    public GameObject suspect1TextGO;
    public GameObject suspect2TextGO;
    public GameObject suspect3TextGO;
    public GameObject suspect1SliderGO;
    public GameObject suspect2SliderGO;
    public GameObject suspect3SliderGO;

    private TextMeshProUGUI suspect1Text;
    private TextMeshProUGUI suspect2Text;
    private TextMeshProUGUI suspect3Text;
    private Slider suspect1Slider;
    private Slider suspect2Slider;
    private Slider suspect3Slider;

    [Header("Sound")]
    public GameObject laboratorySoundGO;
    public AudioClip laboratorySound;
    private AudioSource audioSourceLaboratorySound;

    public GameObject successSoundGO;
    public AudioClip successSound;
    private AudioSource audioSourceSuccessSound;

    [Header("Hints")]
    public int bloodHintsAnalyzed;
    private int bloodHintsAnalyzedRequired;

    public int paintHintsAnalyzed;
    private int paintHintsAnalyzedRequired;

    [Header("Objects to Instantiate")]
    public GameObject bloodGlass;
    public Transform bloodGlassSpot;

    private void Awake()
    {
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
        // Récupération ou ajout des composants
        suspect1Text = GetOrAddComponent<TextMeshProUGUI>(suspect1TextGO);
        suspect2Text = GetOrAddComponent<TextMeshProUGUI>(suspect2TextGO);
        suspect3Text = GetOrAddComponent<TextMeshProUGUI>(suspect3TextGO);
        suspect1Slider = GetOrAddComponent<Slider>(suspect1SliderGO);
        suspect2Slider = GetOrAddComponent<Slider>(suspect2SliderGO);
        suspect3Slider = GetOrAddComponent<Slider>(suspect3SliderGO);

        audioSourceSuccessSound = GetOrAddAudioSource(successSoundGO, successSound);
        audioSourceLaboratorySound = GetOrAddAudioSource(laboratorySoundGO, laboratorySound, true, 0.1f);
        audioSourceLaboratorySound.Play();

        for (int i = 0; i < MuseumManager.Instance.bloodHintsFound; i++)
        {
            Vector3 spawnPosition = bloodGlassSpot.position + new Vector3(0, i * 0.3f, 0);
            Instantiate(bloodGlass, spawnPosition, bloodGlassSpot.rotation, bloodGlassSpot);
        }

        bloodHintsAnalyzedRequired = MuseumManager.Instance.bloodHintsFound;
        paintHintsAnalyzedRequired = MuseumManager.Instance.pictureHintsFound;

        UpdateBloodText();
        UpdatePaintText();
        UpdateSuspectChances();
    }

    private T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
        {
            component = obj.AddComponent<T>();
            Debug.Log($"[LaboratoryManager] {typeof(T).Name} ajouté à {obj.name}");
        }
        return component;
    }

    private AudioSource GetOrAddAudioSource(GameObject targetGO, AudioClip clip, bool loop = false, float volume = 1f)
    {
        AudioSource source = targetGO.GetComponent<AudioSource>();
        if (source == null)
            source = targetGO.AddComponent<AudioSource>();

        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        return source;
    }

    public void GoToMuseum()
    {
        SceneManager.LoadScene("Museum");
    }

    public void AddBloodHintAnalyzed()
    {
        bloodHintsAnalyzed++;
        audioSourceSuccessSound.Play();
        UpdateBloodText();
        UpdateSuspectChances();
    }

    public void AddPaintHintAnalyzed()
    {
        paintHintsAnalyzed++;
        audioSourceSuccessSound.Play();
        UpdatePaintText();
    }

    private void UpdateBloodText()
    {
        var text = GetOrAddComponent<TextMeshProUGUI>(bloodTextObject);
        text.text = $"Traces de sang analysées : {bloodHintsAnalyzed} / {bloodHintsAnalyzedRequired}";
    }

    private void UpdatePaintText()
    {
        var text = GetOrAddComponent<TextMeshProUGUI>(paintTextObject);
        text.text = $"Traces de peintures analysées : {paintHintsAnalyzed} / {paintHintsAnalyzedRequired}";
    }

    private void UpdateSuspectChances()
    {
        float t = Mathf.Clamp01(bloodHintsAnalyzed / 15f); // Révélation complète au bout de 15 indices

        float suspect3 = Mathf.Lerp(33f, 100f, t);
        float remaining = 100f - suspect3;
        float suspect1 = remaining / 2f;
        float suspect2 = remaining / 2f;

        suspect1Text.text = $"Suspect 1 : {suspect1:F0}%";
        suspect2Text.text = $"Suspect 2 : {suspect2:F0}%";
        suspect3Text.text = $"Suspect 3 : {suspect3:F0}%";

        suspect1Slider.value = suspect1 / 100f;
        suspect2Slider.value = suspect2 / 100f;
        suspect3Slider.value = suspect3 / 100f;
    }
}
