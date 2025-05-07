using System.Collections;
using UnityEngine;

public class PoliceInteraction : MonoBehaviour
{
    public GameObject policeWoman;

    [Header("Speech Settings")]
    public RuntimeAnimatorController policeSpeechAnimation;
    public AudioClip policeSpeechAudio;

    [Header("Post-Speech Settings")]
    public RuntimeAnimatorController policeLookAnimation;
    public Vector3 targetRotationEuler;
    public float rotationSpeed = 2.0f;

    [Header("Player")]
    public Transform playerTransform;

    private bool hasTransitioned = false;
    private AudioSource audioSource;
    private bool audioStarted;

    // Initialize the animator and audio source, and start delayed speech
    void Start()
    {
        if (policeSpeechAnimation == null || policeSpeechAudio == null || policeWoman == null || playerTransform == null)
        {
            Debug.LogWarning("Missing references in PoliceInteraction");
            return;
        }

        Animator animator = policeWoman.GetComponent<Animator>();
        if (animator == null)
            animator = policeWoman.AddComponent<Animator>();

        animator.runtimeAnimatorController = policeSpeechAnimation;

        audioSource = policeWoman.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = policeWoman.AddComponent<AudioSource>();

        audioSource.clip = policeSpeechAudio;
    }

    // Rotate towards the player while speaking, then transition animation and rotate to target
    void Update()
    {
        //Start audio
        if(!audioStarted && GameManager.Instance.policeCanStartToTalk)
        {
            StartCoroutine(PlayAudioAfterDelay(2f));
        }

        //Stop audio
        if (GameManager.Instance.isFinished || GameManager.Instance.isAtLab)
        {
            audioSource.Stop();
        }

        if (audioStarted && !hasTransitioned && audioSource.isPlaying)
        {
            Vector3 directionToPlayer = playerTransform.position - policeWoman.transform.position;
            directionToPlayer.y = 0f;

            if (directionToPlayer.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                policeWoman.transform.rotation = Quaternion.Slerp(
                    policeWoman.transform.rotation,
                    lookRotation,
                    Time.deltaTime * rotationSpeed
                );
            }
        }

        if (audioStarted && !hasTransitioned && !audioSource.isPlaying)
        {
            hasTransitioned = true;

            if (policeLookAnimation != null)
            {
                Animator animator = policeWoman.GetComponent<Animator>();
                if (animator == null)
                    animator = policeWoman.AddComponent<Animator>();

                animator.runtimeAnimatorController = policeLookAnimation;
            }

            StartCoroutine(RotateSmoothly(targetRotationEuler));
        }
    }

    // Wait for a delay before playing the speech audio
    private IEnumerator PlayAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
        audioStarted = true;
    }

    // Smoothly rotate the police character to a specific rotation after the speech
    private IEnumerator RotateSmoothly(Vector3 targetRotation)
    {
        Quaternion targetRotationQuat = Quaternion.Euler(targetRotation);
        float timeElapsed = 0f;
        Quaternion currentRotation = policeWoman.transform.rotation;

        while (timeElapsed < 1f)
        {
            policeWoman.transform.rotation = Quaternion.Slerp(currentRotation, targetRotationQuat, timeElapsed);
            timeElapsed += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        policeWoman.transform.rotation = targetRotationQuat;
    }
}
