using System.Collections;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [Header("Sound of pipette taking paint")]
    public GameObject pipetteGO;
    public AudioClip pipetteSound;

    private AudioSource audioSourcePipette;

    // Initialize the pipette audio source and assign the sound clip
    private void Start()
    {
        if (pipetteGO != null && pipetteSound != null)
        {
            audioSourcePipette = pipetteGO.GetComponent<AudioSource>();
            if (audioSourcePipette == null)
                audioSourcePipette = pipetteGO.AddComponent<AudioSource>();

            audioSourcePipette.clip = pipetteSound;
            audioSourcePipette.playOnAwake = false;
        }
    }

    // Detect paint object, register it with GameManager, and play pipette sound
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paint"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddPaintClue();
                other.tag = "Untagged";
            }

            if (audioSourcePipette != null)
                audioSourcePipette.Play();
        }
    }
}
