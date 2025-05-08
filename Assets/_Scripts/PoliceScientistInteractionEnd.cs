using System.Collections.Generic;
using UnityEngine;

public class PeopleManagerEnd : MonoBehaviour
{
    [Header("People")]
    public List<GameObject> peopleToAnimate = new List<GameObject>();

    [Header("Animations")]
    public RuntimeAnimatorController danceAnimation;
    public RuntimeAnimatorController idleAnimation;

    [Header("Sound")]
    public GameObject audioGO;
    private AudioSource audioSource;

    public AudioClip rightSuspectAudio;
    public AudioClip wrongSuspectAudio;
    public AudioClip timeOutAudio;

    private bool hasStarted = false;

    void Start()
    {
        if (peopleToAnimate.Count == 0)
        {
            Debug.LogWarning("No people assigned in PeopleManagerEnd.");
        }

        foreach (GameObject person in peopleToAnimate)
        {
            if (person == null) continue;

            Animator animator = person.GetComponent<Animator>();
            if (animator == null)
                person.AddComponent<Animator>();
        }

        if (audioGO != null)
        {
            audioSource = audioGO.GetComponent<AudioSource>() ?? audioGO.AddComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("audioGO is not assigned.");
        }
    }

    void Update()
    {
        if (hasStarted || GameManager.Instance == null || !GameManager.Instance.isFinished)
            return;

        RuntimeAnimatorController selectedAnimation = null;
        AudioClip selectedClip = null;

        switch (GameManager.Instance.stateGame)
        {
            case 0:
                selectedAnimation = idleAnimation;
                selectedClip = timeOutAudio;
                break;
            case 1:
                selectedAnimation = idleAnimation;
                selectedClip = wrongSuspectAudio;
                break;
            case 2:
                selectedAnimation = danceAnimation;
                selectedClip = rightSuspectAudio;
                break;
            default:
                Debug.LogWarning("Unrecognized stateGame value: " + GameManager.Instance.stateGame);
                return;
        }

        SetAnimationForAll(selectedAnimation);

        if (audioSource != null && selectedClip != null)
        {
            audioSource.clip = selectedClip;
            audioSource.Play();
        }

        hasStarted = true;
    }

    private void SetAnimationForAll(RuntimeAnimatorController controller)
    {
        if (controller == null) return;

        foreach (GameObject person in peopleToAnimate)
        {
            if (person == null) continue;

            Animator animator = person.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = controller;
            }
        }
    }
}
