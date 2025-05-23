using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance { get; private set; }

    [Header("Timer")]
    public GameObject timerTextUI;
    private TimeSpan timer = TimeSpan.FromMinutes(10);
    private bool isCounting = false;

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

    public GameObject introAudioGO;
    public AudioClip introClip;
    private AudioSource introAudio;

    public bool policeCanStartToTalk = false;
    public bool scientistCanStartToTalk = false;

    [Header("Scene & UI Elements")]
    public GameObject player;
    public GameObject museumStartPoint;
    public GameObject labStartPoint;
    public GameObject conclusionStartPoint;
    public List<GameObject> introUI;
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
    public GameObject tubePaintSamplePrefab;
    public Transform tubePaintSampleSpot;

    [Header("Suspect Info")]
    public int correctSuspectIndex = 3;

    [Header("End of the game")]
    public GameObject resultTextUI;
    public int stateGame;
    public bool isFinished = false;
    public bool isAtLab = false;

    #endregion

    // Singleton setup
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Audio initialization and intro speech
    private void Start()
    {
        SetupAudio();
        StartCoroutine(StartIntroAudioDelayed());
    }

    IEnumerator StartIntroAudioDelayed()
    {
        yield return new WaitForSeconds(2f);
        introAudio.Play();
    }

    // Timer control
    private void Update()
    {
        if (isCounting && timer.TotalSeconds > 0)
        {
            timer -= TimeSpan.FromSeconds(Time.deltaTime);

            if (timer.TotalSeconds <= 0)
            {
                timer = TimeSpan.Zero;
                GameResult(0);
            }
        }

        UpdateUIText(timerTextUI, $"{timer.Minutes:D2}:{timer.Seconds:D2}");
    }

    #region Audio

    // Set up audio sources for success, museum, and lab
    private void SetupAudio()
    {
        successAudio = SetupAudioSource(successSoundGO, successClip);
        museumAudio = SetupAudioSource(museumAudioGO, museumClip, true, 1.0f);
        labAudio = SetupAudioSource(labAudioGO, labClip, true, 0.1f);
        introAudio = SetupAudioSource(introAudioGO, introClip);
    }

    // Create and return an AudioSource on the object
    private AudioSource SetupAudioSource(GameObject go, AudioClip clip, bool loop = false, float volume = 1f)
    {
        var source = go.GetComponent<AudioSource>() ?? go.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        return source;
    }

    #endregion

    #region Museum

    // Called when player wants to go to the museum
    public void GoToMuseum()
    {
        isCounting = true;
        policeCanStartToTalk = true;
        EnterMuseum();
    }

    // Teleport player to the lab and initialize museum UI and logic
    private void EnterMuseum()
    {
        player.transform.position = museumStartPoint.transform.position;

        introAudio.Stop();
        museumAudio.Play();
        ToggleUI(introUI, false);
        ToggleUI(museumUI, true);
        museumUI[5].SetActive(false);
        UpdateMuseumUI();
    }

    // Update all clue-related UI text in the museum
    private void UpdateMuseumUI()
    {
        UpdateUIText(salivaClueTextUI, $"Traces de salive rep�r�es avec la lampe UV : {salivaCluesFound} / {salivaCluesRequired}");
        UpdateUIText(footprintClueTextUI, $"Traces de pas photographi�es : {footprintCluesFound} / {footprintCluesRequired}");
        UpdateUIText(paintClueTextUI, $"Traces de peinture collect�es avec la pipette : {paintCluesFound} / {paintCluesRequired}");
    }

    // Called when a saliva clue is collected
    public void AddSalivaClue()
    {
        salivaCluesFound++;
        PlaySuccessSound();
        UpdateMuseumUI();
    }

    // Called when a footprint clue is collected
    public void AddFootprintClue()
    {
        footprintCluesFound++;
        PlaySuccessSound();
        UpdateMuseumUI();
    }

    // Called when a paint clue is collected
    public void AddPaintClue()
    {
        paintCluesFound++;
        PlaySuccessSound();
        UpdateMuseumUI();
    }

    #endregion

    #region Laboratory

    // Called when player wants to go to the lab
    public void GoToLaboratory()
    {
        bool enoughClues = salivaCluesFound + footprintCluesFound + paintCluesFound >= (salivaCluesRequired + footprintCluesRequired + paintCluesRequired) / 2;

        if (enoughClues)
        {
            scientistCanStartToTalk = true;
            EnterLab();
        }
        else
        {
            UpdateUIText(confirmationTextUI, "Vous n'avez pas assez d'indices. Voulez-vous aller au laboratoire quand m�me ?");
            confirmLabButton.SetActive(true);
            goToLabButton.SetActive(false);
        }
    }

    // Called when player confirms they want to go to the lab anyway
    public void ConfirmLabEntry()
    {
        scientistCanStartToTalk = true;
        EnterLab();
    }

    // Teleport player to the lab and initialize lab UI and logic
    private void EnterLab()
    {
        player.transform.position = labStartPoint.transform.position;

        isAtLab = true;
        museumAudio.Stop();
        labAudio.Play();
        ToggleUI(introUI, false);
        ToggleUI(museumUI, false);
        ToggleUI(labUI, true);

        suspect1Slider = GetOrAddComponent<Slider>(suspect1SliderGO);
        suspect2Slider = GetOrAddComponent<Slider>(suspect2SliderGO);
        suspect3Slider = GetOrAddComponent<Slider>(suspect3SliderGO);

        salivaCluesToAnalyze = salivaCluesFound;
        paintCluesToAnalyze = paintCluesFound;

        //Intanciation of Saliva Glasses
        for (int i = 0; i < salivaCluesFound; i++)
        {
            Vector3 pos = salivaSampleSpot.position + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 0.3f * i, UnityEngine.Random.Range(-0.1f, 0.1f));

            Instantiate(salivaSamplePrefab, pos, salivaSampleSpot.rotation, salivaSampleSpot);
        }

        //Intanciation of Tube Paint
        StartCoroutine(SpawnPaintSamplesRoutine());

        UpdateAnalysisUI();
        UpdateSuspectChances();
    }

    private IEnumerator SpawnPaintSamplesRoutine()
    {
        for (int i = 0; i < paintCluesFound; i++)
        {
            Vector3 pos = tubePaintSampleSpot.position + new Vector3(0.4f * i, 0f, 0f);

            Instantiate(tubePaintSamplePrefab, pos, tubePaintSampleSpot.rotation, tubePaintSampleSpot);

            yield return new WaitForSeconds(1f); // attendre 1 seconde avant le prochain spawn
        }
    }

    // Called when a saliva clue is analyzed
    public void AddSalivaAnalysis()
    {
        salivaCluesAnalyzed++;
        PlaySuccessSound();
        UpdateAnalysisUI();
        UpdateSuspectChances();
    }

    // Called when a paint clue is analyzed
    public void AddPaintAnalysis()
    {
        paintCluesAnalyzed++;
        PlaySuccessSound();
        UpdateAnalysisUI();
        UpdateSuspectChances();
    }

    // Update the lab UI text for analysis progress
    private void UpdateAnalysisUI()
    {
        UpdateUIText(salivaAnalysisTextUI, $"Salive analys�e dans le s�quenceur ADN : {salivaCluesAnalyzed} / {salivaCluesToAnalyze}");
        UpdateUIText(paintAnalysisTextUI, $"Peinture analys�e dans la centrifugeuse : {paintCluesAnalyzed} / {paintCluesToAnalyze}");
    }

    // Adjust suspect probability sliders based on clue analysis
    private void UpdateSuspectChances()
    {
        int totalAnalyzed = salivaCluesAnalyzed + paintCluesAnalyzed;
        int totalPossible = 10;

        float t = Mathf.Clamp01((float)totalAnalyzed / totalPossible);
        float guiltyChance = Mathf.Lerp(33f, 100f, t);
        float otherChance = (100f - guiltyChance) / 2f;

        suspect1Slider.value = otherChance / 100f;
        suspect2Slider.value = otherChance / 100f;
        suspect3Slider.value = guiltyChance / 100f;
    }


    // Show buttons to select a suspect
    public void ChooseSuspect()
    {
        buttonLaura.SetActive(true);
        buttonPierre.SetActive(true);
        buttonTheo.SetActive(true);
        buttonConfirmGuilty.SetActive(false);
        UpdateUIText(labConfirmationTextUI, "Choisissez le coupable :");
    }

    // Confirm the selected suspect and show the result
    public void ConfirmSuspectChoice(int chosenIndex)
    {
        bool isCorrect = chosenIndex == correctSuspectIndex;
        string result = isCorrect
            ? "Bravo ! Vous avez trouv� le coupable !"
            : "Zut ! Ce n'�tait pas le bon... Essayez d'analyser plus d'indices la prochaine fois !";
        UpdateUIText(labConfirmationTextUI, result);
    }

    #endregion

    #region Fin

    public void GameResult(int state)
    {
        isCounting = false;
        player.transform.position = conclusionStartPoint.transform.position;
        player.transform.rotation = conclusionStartPoint.transform.rotation;

        isFinished = true;
        museumAudio.Stop();
        labAudio.Stop();
        ToggleUI(introUI, false);
        ToggleUI(museumUI, false);
        ToggleUI(labUI, false);

        if (state == 2)
        {
            stateGame = 2;
            UpdateUIText(resultTextUI, "Bravo ! Vous avez trouv� le coupable !");
        }

        else if (state == 1)
        {
            stateGame = 1;
            UpdateUIText(resultTextUI, "Zut ! Le coupable �tait le num�ro 3 !");
        }
        else
        {
            stateGame = 0;
            UpdateUIText(resultTextUI, "Zut ! Le temps est �coul� !");
        }
    }

    #endregion

    #region Utils

    // Enable or disable UI elements from a list
    private void ToggleUI(List<GameObject> uiElements, bool state)
    {
        foreach (var go in uiElements)
        {
            go.SetActive(state);
        }
    }

    // Utility: Get or add a component to a GameObject
    private T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        return go.GetComponent<T>() ?? go.AddComponent<T>();
    }

    // Update a specific TMP UI text element
    private void UpdateUIText(GameObject obj, string message)
    {
        if (obj == null) return;

        var tmpText = obj.GetComponent<TMP_Text>();
        if (tmpText != null)
        {
            tmpText.text = message;
        }
    }


    // Play the success sound (e.g., after collecting a clue)
    private void PlaySuccessSound()
    {
        if (successAudio != null) successAudio.Play();
    }

    #endregion
}
