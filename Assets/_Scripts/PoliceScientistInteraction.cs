using System.Collections;
using UnityEngine;

public class PoliceScientistInteraction : MonoBehaviour
{
    [Header("People")]
    public GameObject people;
    public bool isPolice;
    public bool isScientist;

    [Header("Speech Settings")]
    public RuntimeAnimatorController speechAnimation;
    public AudioClip speechAudio;

    [Header("Post-Speech Settings")]
    public RuntimeAnimatorController lookAnimation;
    public Vector3 targetRotationEuler;
    public float rotationSpeed = 2.0f;

    [Header("Player")]
    public Transform playerTransform;

    private bool hasTransitioned = false;
    private AudioSource audioSource;
    private bool audioStarted;

    void Start()
    {
        Animator animator = people.GetComponent<Animator>() ?? people.AddComponent<Animator>();
        animator.runtimeAnimatorController = speechAnimation;

        audioSource = people.GetComponent<AudioSource>() ?? people.AddComponent<AudioSource>();
        audioSource.clip = speechAudio;
    }

    void Update()
    {
        if (GameManager.Instance.isAtLab && isPolice)
        {
            audioSource.Stop();
        }

        // Condition d'autorisation de parole selon le type de personnage
        bool canStartToTalk = (isPolice && GameManager.Instance.policeCanStartToTalk)
                           || (isScientist && GameManager.Instance.scientistCanStartToTalk);

        // Lancer l'audio
        if (!audioStarted && canStartToTalk)
        {
            StartCoroutine(PlayAudioAfterDelay(2f));
        }

        // Arrêter l'audio si fin ou changement de scène
        if (GameManager.Instance.isFinished)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }

        // Rotation vers le joueur pendant la parole
        if (audioStarted && !hasTransitioned && audioSource.isPlaying)
        {
            Vector3 directionToPlayer = playerTransform.position - people.transform.position;
            directionToPlayer.y = 0f;

            if (directionToPlayer.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                people.transform.rotation = Quaternion.Slerp(
                    people.transform.rotation,
                    lookRotation,
                    Time.deltaTime * rotationSpeed
                );
            }
        }

        // Transition post-parole
        if (audioStarted && !hasTransitioned && !audioSource.isPlaying)
        {
            hasTransitioned = true;

            Animator animator = people.GetComponent<Animator>() ?? people.AddComponent<Animator>();
            if (lookAnimation != null)
                animator.runtimeAnimatorController = lookAnimation;

            StartCoroutine(RotateSmoothly(targetRotationEuler));
        }
    }

    private IEnumerator PlayAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
        audioStarted = true;
    }

    private IEnumerator RotateSmoothly(Vector3 targetRotation)
    {
        Quaternion targetRotationQuat = Quaternion.Euler(targetRotation);
        float timeElapsed = 0f;
        Quaternion currentRotation = people.transform.rotation;

        while (timeElapsed < 1f)
        {
            people.transform.rotation = Quaternion.Slerp(currentRotation, targetRotationQuat, timeElapsed);
            timeElapsed += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        people.transform.rotation = targetRotationQuat;
    }
}
