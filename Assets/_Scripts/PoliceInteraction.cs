using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class PoliceInteraction : MonoBehaviour
{
    public GameObject policeWoman;

    [Header("Animation & Audio")]
    public AnimatorController policeSpeechAnimation;
    public AudioClip policeSpeechAudio;

    [Header("Post-Speech Settings")]
    public AnimatorController policeLookAnimation;
    public Vector3 targetRotationEuler;
    public float rotationSpeed = 2.0f; // Vitesse de rotation fluide

    private bool hasTransitioned;
    private AudioSource audioSource;
    private bool audioStarted;

    void Start()
    {
        if (policeSpeechAnimation == null || policeSpeechAudio == null || policeWoman == null)
            return;

        // Récupère ou ajoute l'Animator
        Animator animator = policeWoman.GetComponent<Animator>();
        if (animator == null)
            animator = policeWoman.AddComponent<Animator>();

        animator.runtimeAnimatorController = policeSpeechAnimation;

        // Récupère ou ajoute l'AudioSource
        audioSource = policeWoman.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = policeWoman.AddComponent<AudioSource>();

        audioSource.clip = policeSpeechAudio;

        // Démarrer la coroutine pour attendre 3 secondes avant de jouer l'audio
        StartCoroutine(PlayAudioAfterDelay(3f)); // Délai de 3 secondes
    }

    // Coroutine pour attendre 3 secondes avant de jouer l'audio et les animations
    private IEnumerator PlayAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Démarre l'audio après le délai
        audioSource.Play();
        audioStarted = true; // Indiquer que l'audio a commencé
    }

    void Update()
    {
        // N'effectuer la transition que si l'audio a commencé
        if (audioStarted && !hasTransitioned && !audioSource.isPlaying)
        {
            hasTransitioned = true;

            // Changer l'AnimatorController pour l'animation "Look"
            if (policeLookAnimation != null)
            {
                Animator animator = policeWoman.GetComponent<Animator>();
                if (animator == null)
                    animator = policeWoman.AddComponent<Animator>();

                animator.runtimeAnimatorController = policeLookAnimation;
            }

            // Appliquer la rotation fluide (smooth)
            StartCoroutine(RotateSmoothly(targetRotationEuler));
        }
    }

    // Coroutine pour faire tourner la policière de manière fluide
    private IEnumerator RotateSmoothly(Vector3 targetRotation)
    {
        Quaternion targetRotationQuat = Quaternion.Euler(targetRotation);
        float timeElapsed = 0f;
        Quaternion currentRotation = policeWoman.transform.rotation;

        // Lerp jusqu'à ce qu'on atteigne la rotation cible
        while (timeElapsed < 1f)
        {
            policeWoman.transform.rotation = Quaternion.Slerp(currentRotation, targetRotationQuat, timeElapsed);
            timeElapsed += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        // S'assurer que la rotation finale est bien la cible
        policeWoman.transform.rotation = targetRotationQuat;
    }
}
