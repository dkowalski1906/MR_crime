using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements - Museum")]
    public GameObject salivaClueTextUI;
    public GameObject footprintClueTextUI;
    public GameObject paintClueTextUI;
    public GameObject confirmationTextUI;
    public GameObject confirmLabButton;
    public GameObject goToLabButton;

    [Header("UI Elements - Laboratory")]
    public GameObject salivaAnalysisTextUI;
    public GameObject paintAnalysisTextUI;
    public GameObject labConfirmationTextUI;
    public GameObject buttonLaura;
    public GameObject buttonPierre;
    public GameObject buttonTheo;
    public GameObject buttonConfirmGuilty;

    [Header("Clues Found")]
    public int salivaCluesFound;
    public int salivaCluesRequired;
    public int footprintCluesFound;
    public int footprintCluesRequired;
    public int paintCluesFound;
    public int paintCluesRequired;

    [Header("Clues Analyzed")]
    public int salivaCluesAnalyzed;
    private int salivaCluesToAnalyze;
    public int paintCluesAnalyzed;
    private int paintCluesToAnalyze;

    [Header("Audio")]
    public GameObject successSoundGO;
    public AudioClip successClip;
    private AudioSource successAudio;

    public GameObject museumAudioGO;
    public AudioClip museumClip;
    private AudioSource museumAudio;

    public GameObject labAudioGO;
    public AudioClip labClip;
    private AudioSource labAudio;

    [Header("Scene & UI Elements")]
    public GameObject player;
    public GameObject labStartPoint;
    public List<GameObject> museumUI;
    public List<GameObject> labUI;

    [Header("Sliders - Suspects")]
    public GameObject suspect1SliderGO;
    public GameObject suspect2SliderGO;
    public GameObject suspect3SliderGO;
    private Slider suspect1Slider;
    private Slider suspect2Slider;
    private Slider suspect3Slider;

    [Header("Instantiations")]
    public GameObject salivaSamplePrefab;
    public Transform salivaSampleSpot;

    [Header("Suspect Info")]
    public int correctSuspectIndex = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetupAudio();
        UpdateMuseumUI();
    }

    private void SetupAudio()
    {
        successAudio = SetupAudioSource(successSoundGO, successClip);
        museumAudio = SetupAudioSource(museumAudioGO, museumClip, true, 1.0f);
        labAudio = SetupAudioSource(labAudioGO, labClip, true, 0.1f);
        museumAudio.Play();
    }

    private AudioSource SetupAudioSource(GameObject go, AudioClip clip, bool loop = false, float volume = 1f)
    {
        var source = go.GetComponent<AudioSource>() ?? go.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        return source;
    }

    private void UpdateMuseumUI()
    {
        UpdateUIText(salivaClueTextUI, $"Traces de salive collectées : {salivaCluesFound} / {salivaCluesRequired}");
        UpdateUIText(footprintClueTextUI, $"Traces de pas collectées : {footprintCluesFound} / {footprintCluesRequired}");
        UpdateUIText(paintClueTextUI, $"Traces de peinture collectées : {paintCluesFound} / {paintCluesRequired}");
    }

    private void UpdateUIText(GameObject obj, string message)
    {
        if (obj != null)
        {
            var text = obj.GetComponent<TextMeshPro>();
            if (text != null) text.text = message;
        }
    }

    private void PlaySuccessSound()
    {
        if (successAudio != null) successAudio.Play();
    }

    public void AddSalivaClue()
    {
        salivaCluesFound++;
        PlaySuccessSound();
        UpdateMuseumUI();
    }

    public void AddFootprintClue()
    {
        footprintCluesFound++;
        PlaySuccessSound();
        UpdateMuseumUI();
    }

    public void AddPaintClue()
    {
        paintCluesFound++;
        PlaySuccessSound();
        UpdateMuseumUI();
    }

    public void GoToLaboratory()
    {
        bool enoughClues = salivaCluesFound + footprintCluesFound >= (salivaCluesRequired + footprintCluesRequired) / 2;

        if (enoughClues)
        {
            EnterLab();
        }
        else
        {
            UpdateUIText(confirmationTextUI, "Vous n'avez pas assez d'indices. Voulez-vous aller au laboratoire quand même ?");
            confirmLabButton?.SetActive(true);
            goToLabButton?.SetActive(false);
        }
    }

    public void ConfirmLabEntry()
    {
        EnterLab();
    }

    private void EnterLab()
    {
        player.transform.position = labStartPoint.transform.position;

        museumAudio.Stop();
        labAudio.Play();
        ToggleUI(museumUI, false);
        ToggleUI(labUI, true);

        suspect1Slider = GetOrAddComponent<Slider>(suspect1SliderGO);
        suspect2Slider = GetOrAddComponent<Slider>(suspect2SliderGO);
        suspect3Slider = GetOrAddComponent<Slider>(suspect3SliderGO);

        for (int i = 0; i < salivaCluesFound; i++)
        {
            Vector3 pos = salivaSampleSpot.position + new Vector3(Random.Range(-0.1f, 0.1f), 0.3f * i, Random.Range(-0.1f, 0.1f));
            Instantiate(salivaSamplePrefab, pos, salivaSampleSpot.rotation, salivaSampleSpot);
        }

        salivaCluesToAnalyze = salivaCluesFound;
        paintCluesToAnalyze = paintCluesFound;

        UpdateAnalysisUI();
        UpdateSuspectChances();
    }

    public void AddSalivaAnalysis()
    {
        salivaCluesAnalyzed++;
        PlaySuccessSound();
        UpdateAnalysisUI();
        UpdateSuspectChances();
    }

    public void AddPaintAnalysis()
    {
        paintCluesAnalyzed++;
        PlaySuccessSound();
        UpdateAnalysisUI();
    }

    private void UpdateAnalysisUI()
    {
        UpdateUIText(salivaAnalysisTextUI, $"Traces de salive analysées : {salivaCluesAnalyzed} / {salivaCluesToAnalyze}");
        UpdateUIText(paintAnalysisTextUI, $"Traces de peinture analysées : {paintCluesAnalyzed} / {paintCluesToAnalyze}");
    }

    private void UpdateSuspectChances()
    {
        float t = Mathf.Clamp01(salivaCluesAnalyzed / 15f);
        float guiltyChance = Mathf.Lerp(33f, 100f, t);
        float otherChance = (100f - guiltyChance) / 2f;

        suspect1Slider.value = otherChance / 100f;
        suspect2Slider.value = otherChance / 100f;
        suspect3Slider.value = guiltyChance / 100f;
    }

    public void ChooseSuspect()
    {
        buttonLaura.SetActive(true);
        buttonPierre.SetActive(true);
        buttonTheo.SetActive(true);
        buttonConfirmGuilty.SetActive(false);
        UpdateUIText(labConfirmationTextUI, "Choisissez le coupable :");
    }

    public void ConfirmSuspectChoice(int chosenIndex)
    {
        bool isCorrect = chosenIndex == correctSuspectIndex;
        string result = isCorrect
            ? "Bravo ! Vous avez trouvé le coupable !"
            : "Zut ! Ce n'était pas le bon... Essayez d'analyser plus d'indices la prochaine fois !";
        UpdateUIText(labConfirmationTextUI, result);
    }

    private void ToggleUI(List<GameObject> uiElements, bool state)
    {
        foreach (var go in uiElements)
        {
            go.SetActive(state);
        }
    }

    private T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        return go.GetComponent<T>() ?? go.AddComponent<T>();
    }
}
