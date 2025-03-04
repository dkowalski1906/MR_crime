using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //test bouton
    public Button monBouton1;
    public Button monBouton2;

    //spawn robot
    public Transform spawnRobot;
    private GameObject robot;

    //spawn object
    public Transform spawnObject;
    public GameObject blue;
    public GameObject red;
    public GameObject green;
    public GameObject yellow;
    public GameObject purple;
    public GameObject orange;
    public GameObject headRobot1;
    public GameObject headRobot2;
    public GameObject bodyRobot1;
    public GameObject bodyRobot2;
    private Vector3 offsetspawnObject = new Vector3(0f, 0f, 0f);
    private GameObject material;

    //step
    private bool validateStep;
    public int currentStep = 1;

    //avatars
    public GameObject avatar1;

    //voices
    public AudioSource IntroVoice;
    public AudioSource HandMenuVoice;
    public AudioSource GrabObjectVoice;
    public AudioSource ProcessVoice;
    public AudioSource leverSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Invoke("SpawnRobot", 0f);
        // monBouton1.onClick.Invoke();
        // monBouton2.onClick.Invoke();

        //introduction voice
        IntroVoice.Play();
    }

    void update()
    {
        //avatar1.transform.localRotation = Quaternion.Euler(0,rotation,0);
    }

    public void SpawnRobot()
    {
        robot = new GameObject("Robot");
        robot.tag = "robot";
        robot.transform.position = spawnRobot.position;
    }

    public void SpawnObject(string objectName)
    {
        Vector3 spawnPosition = spawnObject.position + spawnObject.TransformDirection(offsetspawnObject);

        GameObject prefab = objectName switch
        {
            "blue" => blue,
            "red" => red,
            "green" => green,
            "yellow" => yellow,
            "purple" => purple,
            "orange" => orange,
            "head2" => headRobot2,
            "head1" => headRobot1,
            "body1" => bodyRobot1,
            "body2" => bodyRobot2,
            _ => null
        };

        if (prefab != null)
        {
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Unknown object name or variant: {objectName}");
        }
    }

    public void ReStartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void HandMenuVoiceActivated()
    {
        leverSound.Play();
        HandMenuVoice.Play();
    }

    public void HandMenuVoiceDesactivated()
    {
        leverSound.Play();
        HandMenuVoice.Stop();
    }

    public void GrabObjectVoiceActivated()
    {
        leverSound.Play();
        GrabObjectVoice.Play();
    }

    public void GrabObjectVoiceDesactivated()
    {
        leverSound.Play();
        GrabObjectVoice.Stop();
    }

    public void ProcessVoiceActivated()
    {
        leverSound.Play();
        ProcessVoice.Play();
    }

    public void ProcessVoiceDesactivated()
    {
        leverSound.Play();
        ProcessVoice.Stop();
    }
}
